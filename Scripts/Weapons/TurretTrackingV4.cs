using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Turret tracking system which can be extended to calculate interception course.
/// </summary>
public class TurretTrackingV4 : MonoBehaviour
{
    public Transform horizontalRotation;
    public Transform verticalRotation;
    public Transform barrelRotation;

    public bool rotatingBarrel = false;
    public float rotationSpeed = 10.0f;
    public float barrelSpeed = 800.0f;
    public float horizontalSpeed = 90f;
    public float verticalSpeed = 90f;
    public float followMultiplyer = 4f;

    GameObject _targets;

    Vector3 m_lastKnownPosition = Vector3.zero;
    Quaternion m_lookAtRotation;

    Quaternion _verticalDefaultPos;
    Quaternion _horizontalDefaultPos;

    void Start()
    {
        _verticalDefaultPos = verticalRotation.localRotation;
        _horizontalDefaultPos = horizontalRotation.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (_targets)
        {
            // If the target is moving update rotation
            if (_targets.transform.position != m_lastKnownPosition)
            {
                m_lastKnownPosition = _targets.transform.position;
                m_lookAtRotation = Quaternion.LookRotation(m_lastKnownPosition - verticalRotation.position);
            }

            // If turret is not facing the target rotate towards it
            if (verticalRotation.rotation != m_lookAtRotation)
            {
                HorizontalRotation();
                VerticalRotation();
            }
            if(rotatingBarrel)
            {
                barrelRotation.Rotate(0, 0, barrelSpeed * Time.deltaTime);
            }
        }
        else
        {
            DefaultPosition();
        }
    }
    private void DefaultPosition()
    {
        Vector3 hRotation = Quaternion.Lerp(horizontalRotation.rotation, _horizontalDefaultPos, horizontalSpeed * Time.deltaTime).eulerAngles;
        horizontalRotation.rotation = Quaternion.Euler(0f, hRotation.y, 0f);

        Vector3 vRotation = Quaternion.Lerp(verticalRotation.localRotation, _verticalDefaultPos, verticalSpeed * Time.deltaTime).eulerAngles;
        verticalRotation.localRotation = Quaternion.Euler(vRotation.x, 0f, 0f);
    }
    // TODO: Should rotate according to Spawners location for accurate aiming
    private void HorizontalRotation()
    {
        // Find the direction from our position to target position
        Vector3 direction = _targets.transform.position - transform.position;
        // How do we need to rotate ourselves to look in that direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        // Convert Quaterion to eurelAngles (thats what we see in the editor)
        Vector3 rotation = Quaternion.Lerp(horizontalRotation.rotation, targetRotation, horizontalSpeed * followMultiplyer * Time.deltaTime).eulerAngles;

        horizontalRotation.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
    private void VerticalRotation()
    {
        Vector3 direction = _targets.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(verticalRotation.localRotation, targetRotation, verticalSpeed * followMultiplyer * Time.deltaTime).eulerAngles;

        verticalRotation.localRotation = Quaternion.Euler(rotation.x, 0f, 0f);
    }

    public void SetTarget(GameObject target)
    {
        _targets = target;
    }
    // https://answers.unity.com/questions/296949/how-to-calculate-a-position-to-fire-at.html
    // https://stackoverflow.com/questions/17204513/how-to-find-the-interception-coordinates-of-a-moving-target-in-3d-space
    public Vector3 CalculateInterceptCourse(Vector3 aTargetPos, Vector3 aTargetSpeed, Vector3 aInterceptorPos, float aInterceptorSpeed)
    {
        Vector3 targetDir = aTargetPos - aInterceptorPos;
        float iSpeed2 = aInterceptorSpeed * aInterceptorSpeed;
        float tSpeed2 = aTargetSpeed.sqrMagnitude;
        float fDot1 = Vector3.Dot(targetDir, aTargetSpeed);
        float targetDist2 = targetDir.sqrMagnitude;
        float d = (fDot1 * fDot1) - targetDist2 * (tSpeed2 - iSpeed2);
        if (d < 0.1f)  // negative == no possible course because the interceptor isn't fast enough
            return Vector3.zero;
        float sqrt = Mathf.Sqrt(d);
        float S1 = (-fDot1 - sqrt) / targetDist2;
        float S2 = (-fDot1 + sqrt) / targetDist2;
        if (S1 < 0.0001f)
        {
            if (S2 < 0.0001f)
                return Vector3.zero;
            else
                return (S2) * targetDir + aTargetSpeed;
        }
        else if (S2 < 0.0001f)
            return (S1) * targetDir + aTargetSpeed;
        else if (S1 < S2)
            return (S2) * targetDir + aTargetSpeed;
        else
            return (S1) * targetDir + aTargetSpeed;
    }

    //first-order intercept using absolute target position
    public static Vector3 FirstOrderIntercept
    (
        Vector3 shooterPosition,
        Vector3 shooterVelocity,
        float shotSpeed,
        Vector3 targetPosition,
        Vector3 targetVelocity
    )
    {
        Vector3 targetRelativePosition = targetPosition - shooterPosition;
        Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
        float t = FirstOrderInterceptTime
        (
            shotSpeed,
            targetRelativePosition,
            targetRelativeVelocity
        );
        return targetPosition + t * (targetRelativeVelocity);
    }
    //first-order intercept using relative target position
    public static float FirstOrderInterceptTime
    (
        float shotSpeed,
        Vector3 targetRelativePosition,
        Vector3 targetRelativeVelocity
    )
    {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if (velocitySquared < 0.001f)
            return 0f;

        float a = velocitySquared - shotSpeed * shotSpeed;

        //handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude /
            (
                2f * Vector3.Dot
                (
                    targetRelativeVelocity,
                    targetRelativePosition
                )
            );
            return Mathf.Max(t, 0f); //don't shoot back in time
        }

        float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        { //determinant > 0; two intercept paths (most common)
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                    t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //both are positive
                else
                    return t1; //only t1 is positive
            }
            else
                return Mathf.Max(t2, 0f); //don't shoot back in time
        }
        else if (determinant < 0f) //determinant < 0; no intercept path
            return 0f;
        else //determinant = 0; one intercept path, pretty much never happens
            return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
    }
}
