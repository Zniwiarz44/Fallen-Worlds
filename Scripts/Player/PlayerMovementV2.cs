using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;         // Required to use events
using UnityEngine.AI;

public class PlayerMovementV2 : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public float inputHoldDelay = 0.5f;                                         // How long the input is on hold while we are interacting
    public float turnSpeedTreshold = 0.5f;                                      // Speed required to start turning
    public float speedDampTime = 0.1f;                                          // Time over which speed will change to a value that we set it
    public float slowingSpeed = 0.175f;
    public float turnSmoothing = 15f;                                           // Lower value = smoother

    private WaitForSeconds inputHoldWait;
    private Vector3 destinationPosition;
    private Interactable currentInteractable;                                   // Player can interact with the world
    private bool handleInput = true;                                            // Block movement while interacting
   // private GameObject currentTarget = null;                                    // Target to engage

    private const float stopDistanceProportion = 0.1f;                          // If we are 10% from the stopping distance
    private const float navMeshSampleDistance = 4f;                             // Distance away from the click that navMesh can be

    private readonly int hashSpeedPara = Animator.StringToHash("Speed");        // Speed has to match the name we defined in the Animator
    private readonly int hashLocomotionTag = Animator.StringToHash("Locomotion");

    //-----<Combat Movement>-----
    public bool InCombat { get { return inCombat; } }
    public GameObject CurrentTarget { get { return curTarget; } }
    private bool inCombat = false;                                              // Change player movement to combat

    private Vector2 smoothDeltaPosition = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    private GameObject curTarget = null;

    //private int hashInCombatPara = Animator.StringToHash("InCombat");

    // Use this for initialization
    void Start()
    {
        // Rotate player rotation manually, if true navMesh will do that automatically
        agent.updateRotation = false;

        // Stop the player from moving and performing any other actions while interacting
        inputHoldWait = new WaitForSeconds(inputHoldDelay);

        destinationPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Wait while calculating the path
        if (agent.pathPending)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CombatDisengage();
        }
        // Calculate how fast we want to move based on navMesh desired velocity, there are 2 velocities, current and another at which navMesh wants to go
        float speed = agent.desiredVelocity.magnitude;
        if (inCombat)
        {
            CombatMovement();
        }
        else
        {
            // Default stoppingDistance is 0.15f
            // 0.15f * 0.1f = 0.015f This value is lower than agent.stoppingDistance which is 0.15f
            if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion)
            {
                //Debug.Log("Stopping!");
                Stopping(out speed);
            }
            else if (agent.remainingDistance <= agent.stoppingDistance)
            {
                //Debug.Log("Slowing!");
                Slowing(out speed, agent.remainingDistance);
            }
            else if (speed > turnSpeedTreshold)
            {
                Moving();
            }
        }

        animator.SetFloat(hashSpeedPara, speed, speedDampTime, Time.deltaTime);
        //Debug.Log("Dist " + agent.remainingDistance + "\nstop " + agent.stoppingDistance * 40f);
    }

    /// <summary>
    ///  Determines how character is going to move
    /// </summary>
    private void OnAnimatorMove()
    {
        // Technically NavMesh will determine how fast the character is moving
        // Speed = Distance / Time
        if (inCombat)
        {
            transform.position = agent.nextPosition;
        }
        else
        {
            agent.velocity = animator.deltaPosition / Time.deltaTime;
        }
    }

    #region NormalMovement
    /// <summary>
    /// Determine when we need to start stopping
    /// </summary>
    /// <param name="speed"></param>
    private void Stopping(out float speed)  // Affect the speed within the function
    {
        agent.isStopped = true;

        // Snap to the place we are aiming for
        transform.position = destinationPosition;
        speed = 0f;
        if (currentInteractable)
        {
            // Facing the interaction
            transform.rotation = currentInteractable.interactionLocation.rotation;

            //---<Interaction>---
            currentInteractable.Interact();
            // Make sure we only click once
            currentInteractable = null;
            // Since we are interacting block the input to avoid player movement
            StartCoroutine(WaitForInteraction());
        }
    }
    // Slowing based on how far from our destination we are
    private void Slowing(out float speed, float distanceToDestination)
    {
        agent.isStopped = true;
        // Gradually move player to his destination
        transform.position = Vector3.MoveTowards(transform.position, destinationPosition, slowingSpeed * Time.deltaTime);

        // How close to the destination we are to the stopping distance? Then calculate the speed
        float proportionalDistance = 1f - distanceToDestination / agent.stoppingDistance;
        // Interpolate
        speed = Mathf.Lerp(slowingSpeed, 0f, proportionalDistance);
        //Debug.Log("Distance " + proportionalDistance);
        //---<Interaction>---
        // We don't want to snap the rotation when approaching from a different angle
        // If we are going for the interaction rotate towards, else keep rotation
        Quaternion targetRotation = currentInteractable ? currentInteractable.interactionLocation.rotation : transform.rotation;

        // Interpolate towards target rotation, rotation speed is based on proportionalDistance
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, proportionalDistance);
    }

    private void Moving()
    {
        // Target rotation
        Quaternion targetRotation = Quaternion.LookRotation(agent.desiredVelocity);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
    }

    // Select Level with navmesh on and in the Event Trigger select this function
    public void OnGroundClick(BaseEventData data)
    {

        if (!handleInput)
        {
            return;
        }
        // If we clicked interactable and changed our mind then set interactable to null
        currentInteractable = null;

        PointerEventData pData = (PointerEventData)data;
        // Find point on the navMesh that's closes to our click
        NavMeshHit hit;       
        // Para 1: point in the world we want to check, PhysicsRayCast component that provides the data
        // Para 2: what we hit
        // Para 3: distance
        // Para 4: All areas of that mesh
        if (NavMesh.SamplePosition(pData.pointerCurrentRaycast.worldPosition, out hit, navMeshSampleDistance, NavMesh.AllAreas))
        {
            destinationPosition = hit.position;
        }
        else
        {
            // Player will try to get to the area that the player hit
            destinationPosition = pData.pointerCurrentRaycast.worldPosition;
        }
        agent.SetDestination(destinationPosition);
        agent.isStopped = false;
    }
    // Which interactable we clicked on
    public void OnInteractableClick(Interactable interactable)
    {
        // If we can't handle input return, if we are already interacting
        if (!handleInput)
        {
            return;
        }

        currentInteractable = interactable;
        destinationPosition = currentInteractable.interactionLocation.position;

        // Go towards interactable/clicked position
        agent.SetDestination(destinationPosition);
        agent.isStopped = false;
    }

    // This coroutine determines how long the interaction will last
    private IEnumerator WaitForInteraction()
    {
        handleInput = false;

        // yield statement exits the code and waits for the thing to be true and then comes back in
        yield return inputHoldWait;

        // While our current state is not one that has locomotion tag
        while (animator.GetCurrentAnimatorStateInfo(0).tagHash != hashLocomotionTag)
        {
            // Every frame that we are not in the locomotion state, is entering the while loop, then is waiting frame and checking again 
            // until we have the locomotion tag and then go to handleInput
            yield return null;
        }

        handleInput = true;
    }
    #endregion

    #region InCombat
    public void OnCombatEngage(GameObject newTarget)
    {
        Debug.Log("In Combat ");
        curTarget = newTarget;
        animator.SetBool("InCombat", true);
        animator.SetTrigger("FightMode");
        inCombat = true;
        //transform.LookAt(newTarget.transform);

        agent.updatePosition = false;
    }

    public void CombatDisengage()
    {
        inCombat = false;
        animator.SetBool("InCombat", false);
        curTarget = null;
        agent.updatePosition = true;
        Debug.Log("Combat abandoned");
    }
    public void CombatMovement()
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
        animator.SetFloat("velx", velocity.x);
        animator.SetFloat("vely", velocity.y);

        //Debug.Log("Velx: " + velocity.x + " \nVely: " + velocity.y);

        RotateTowardsEnemy();
    }
    // TODO: Implement a corutine
    void RotateTowardsEnemy()
    {
        // Player is probably locked to rotate around y axis because of Navmesh
        if (curTarget != null)
        {
            // TODO: Create smooth rotation
            //transform.LookAt(curTarget.transform);

            Vector3 lookAtDirection = (curTarget.transform.position - transform.position).normalized;
            float targetAngle = Mathf.Atan2(lookAtDirection.x, lookAtDirection.z) * Mathf.Rad2Deg;

            // DeltaAngle returns the difference between 2 angles, if we are close then stop rotating
            while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
            {
                float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeedTreshold * Time.deltaTime);
                transform.eulerAngles = Vector3.up * angle;
            }
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, curTarget.transform.rotation, turnSpeedTreshold * Time.deltaTime);
        }
        else
        {
            CombatDisengage();
        }
    }
    #endregion
}
