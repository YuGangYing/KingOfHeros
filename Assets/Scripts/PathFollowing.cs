//----------------------------------------------------------------------------------
// The example script to follow path. 
// It manages waypointed path from pathFindingScript and move object along it.
//----------------------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PathFollowing : MonoBehaviour
{
    public PathFinding pathFindingScript;				// Path holder/generator script
    public float damping = 10.0f;								// Smooth facing/movement value
    public float movementSpeed= 5.0f;					// Speed of object movement along the path
    public float waypointActivationDistance= 3.0f;	// How far should object be to waypoint for its activation and choosing new
    public float stuckDistance= 2f;                   // Max distance of move per regenTimeout that supposed to indicate stuking
    public float stuckTimeout= 2f;                    // How fast should path be regenerated if player stucks

    // Usefull internal variables, please don't change them blindly
    private int currentWaypoint = 0;
    private Vector3 targetPosition;
    private bool inMove = false;

    private Vector3 oldPosition;
    private float timeToRegen;
    
    //=============================================================================================================
    // Setup initial data according to specified parameters
    void Start()
    {
        // Make the rigid body not change rotation
//         if (rigidbody)
//             rigidbody.freezeRotation = true;

        pathFindingScript = GetComponent<PathFinding>();
    }
    
    //----------------------------------------------------------------------------------
    //Main loop
    void Update()
    {
        if (pathFindingScript.target == null && pathFindingScript.bTargetPosition == false)
            return;

        // Check if object is near target point
        if ((transform.position - pathFindingScript.TargetPosition).sqrMagnitude < waypointActivationDistance * waypointActivationDistance)
        {
            if (inMove) 
            {
                pathFindingScript.FindPath();
                inMove = false;
                pathFindingScript.target = null;
            }
        }
        else
        {
            // Try to get next waypoint. If it is missed in some reason - set currentWaypoint to 1
            try
            {
                targetPosition = pathFindingScript.waypoints[currentWaypoint];
            }
            catch(System.Exception /*ex*/)
            {
                currentWaypoint = 1;
            }
            
            // Activate waypoint when object is closer than waypointActivationDistance
            if ((transform.position - targetPosition).sqrMagnitude < waypointActivationDistance * waypointActivationDistance)
            {
                if (currentWaypoint > pathFindingScript.waypoints.Count-1)
                    currentWaypoint = 1;
                currentWaypoint ++;
            }

            // Look at and dampen the rotation
            Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            transform.Translate(Vector3.forward*movementSpeed*Time.deltaTime);
            
            inMove = true;
            if (Time.time > timeToRegen)
            {
                if ((transform.position-oldPosition).sqrMagnitude < stuckDistance * stuckDistance) 
                {
                    pathFindingScript.FindPath();
                    currentWaypoint = 1;
                }
                
                oldPosition = transform.position; 
                timeToRegen = Time.time + stuckTimeout;
            }
        }
    }
    
    //----------------------------------------------------------------------------------
    // Return true if object is moving now
    bool isMoving()
    {
        return inMove;
    }
    //----------------------------------------------------------------------------------
}