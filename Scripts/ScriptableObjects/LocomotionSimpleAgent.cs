using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class LocomotionSimpleAgent : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    GameObject curTarget = null;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        // Don’t update position automatically
        agent.updatePosition = false;
        agent.updateRotation = true;
    }

    void Update()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);

        // Determine where we clicked in the world in relation to the character
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        //bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

        // Update animation parameters

        //anim.SetBool("InCombat", shouldMove);
        anim.SetFloat("velx", velocity.x);
        anim.SetFloat("vely", velocity.y);

        RotateTowardsEnemy();
        // GetComponent<LookAt>().lookAtTargetPosition = agent.steeringTarget + transform.forward;
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
    }

    public void OnCombatEngage(GameObject newTarget)
    {
        Debug.Log("In Combat ");
        // TODO: Trigger FightMode
        curTarget = newTarget;
        anim.SetBool("InCombat", true);
        anim.SetTrigger("FightMode");
        //transform.LookAt(newTarget.transform);
    }

    void RotateTowardsEnemy()
    {
        if (curTarget)
        {
            // TODO: Create smooth rotation
            transform.LookAt(curTarget.transform);
            
            //Quaternion targetRotation = Quaternion.LookRotation(curTarget.transform.position);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 15f * Time.deltaTime);
        }
    }
}