using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RangeChecker))]
[RequireComponent(typeof(TurretTrackingV4))]
[RequireComponent(typeof(Weapon))]

public class TurretAI : MonoBehaviour
{

    public enum AIStates { NEAREST };
    public AIStates aiStates = AIStates.NEAREST;

    RangeChecker _range;
    TurretTrackingV4 _tracking;
    Weapon _weapon;

    // Use this for initialization
    void Start()
    {
        _range = GetComponent<RangeChecker>();
        _tracking = GetComponent<TurretTrackingV4>();
        _weapon = GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (aiStates)
        {
            case AIStates.NEAREST:
                TargetNearest();
                break;
        }
    }

    void TargetNearest()
    {
        List<GameObject> validTargets = _range.GetValidTargets();
        GameObject currentTarget = null;
        float closestDist = 0.0f;
        
        for (int i = 0; i < validTargets.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, validTargets[i].transform.position);

            // if there is no target or other object is closer
            if (!currentTarget || dist < closestDist)
            {
                currentTarget = validTargets[i];
                closestDist = dist;
            }
        }
        // Tracking is expecting null to reset to default
        _tracking.SetTarget(currentTarget);
       
        if (currentTarget != null)
        {
            _weapon.SetTarget(currentTarget);
            _weapon.Shoot();
        }
    }
}
