//-----------------------------------------------------------------------------------------------
// Main script of this Path finding system. 
// Calculates(find) path automatically or according to specified rules
// Generates array of waypoints (around obstacles) until target will be reached      
//-----------------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PathFinding : MonoBehaviour
{
    [SerializeField]
    private Transform Target;						// Target/final point to build path to

    public Transform target
    {
        get { return Target; }
        set
        {
            Target = value;
        }
    }

    public bool bTargetPosition = false;
    Vector3 targetPosition;
    public void SetTargetPositon(Vector3 tp)
    {
        bTargetPosition = true;
        targetPosition = tp;
    }

    public void ClearTargetPosition()
    {
        bTargetPosition = false;
    }

    public Vector3 TargetPosition
    {
        get
        {
            if (bTargetPosition == false)
            {
                if (target != null)
                    return target.position;

                return transform.position;
            }
            else
            {
                return targetPosition;
            }
        }
    }

    [HideInInspector]
    public List<Vector3> waypoints = new List<Vector3>();			        // Array of generated waypoints

    public int maxComplexity = 15;				// Max number of waypoins in path
    public float maxLookingDistance = 50.0f;		// Max distance of raycasts
    public float offsetFromObstacles = 1f;			// Set additional offset between waypoints and  obstacles
    public float autoUpdateTime;					// Delay to next path recalculation. Works  automatically if updateOnTargetMove & manualUpdateOnly = false;
    public bool updateOnTargetMove = true;		// Update only if new target position different  from previous one
    public bool manualUpdateOnly = false;		// Allow only manual updates  by calling "FindPath" function
    public bool useZAxisAsHeight = false;      // By default path calculates in XZ plane, set it to true to use XY plane
    public bool ignoreTargetHeight = true;     // Ignore target Y (or Z) offset from this object 
    public Color color = new Color(0, 1f, 0f, 0.5f);       // Debug path-visualization color

    public bool noPathFound = false;

    // Usefull internal variables, please don't change them blindly
    private float recalculatePathTime;
    private Vector3 oldTargetPosition;
    private bool wasRecalculated;

    //================================================================================================
    // Find path on object creation
    void Start () 
    {
        if (target != null)
            FindPath ();
    }
    
    //----------------------------------------------------------------------------------
    // Makes path finding automatically, according to specified rules
    void Update () 
    {
        if (target == null && bTargetPosition == false)
            return;

        if(!manualUpdateOnly)
        {
            // Update only if new target position different from previous one
            if (updateOnTargetMove)
            {
                if (TargetPosition != oldTargetPosition)
                    // and only if autoUpdateTime passed
                    if (Time.time > recalculatePathTime)
                    {
                        FindPath ();
                        recalculatePathTime = Time.time + autoUpdateTime;
                        oldTargetPosition = TargetPosition;
                    }
            }
            else if (Time.time > recalculatePathTime)// Update by timer
            {
                FindPath ();
                recalculatePathTime = Time.time + autoUpdateTime;
            }
        }
    }
    
    //----------------------------------------------------------------------------------
    // Recalculate path. Main function that finds path and generate waypoints
    // Please also use this function to manually initiate FindPath procedure
    public void FindPath()
    {
        // Usefull internal variables, please don't change them blindly
        RaycastHit hit;
        Ray ray;
        Vector3 pos;
        Vector3 dir;
        Vector3 raycastedPoint;
        float lookingDistance;
        Vector3 targetPosition;

        // Reset path
        waypoints.Clear();
        waypoints.Add(transform.position);
        
        lookingDistance = maxLookingDistance;
        targetPosition = TargetPosition;
	 
        // Ignore Targets height and assign vertical axis
        if (ignoreTargetHeight)
            if (useZAxisAsHeight)
                targetPosition.z = transform.position.z;
            else
                targetPosition.y = transform.position.y;

        // Set start point and direction            
        pos = waypoints[waypoints.Count-1]; 
        dir = targetPosition - pos;

        noPathFound = false;

        // Main loop. Generate waypoints around obstacles until target be  reached (or waypoints quantity become bigger than maxComplexity)
        while(waypoints[waypoints.Count-1] != targetPosition  && (waypoints.Count < maxComplexity) ) 
        {
            ray = new Ray (pos, dir);

            // Raycast from last finded waypoint to choosed direction (straight to target or around current obstacle)
            if (Physics.Raycast(ray, out hit, lookingDistance)) 
            {
                // If there is  obstacle - create new  waypoint in front of it
                raycastedPoint = ray.GetPoint(hit.distance-offsetFromObstacles);
                waypoints.Add(raycastedPoint);

				// Calculate normal around obstacle (taking(or not) into account vertical coordinates)
			    if (useZAxisAsHeight)
                {
                    dir.x = hit.normal.x * Mathf.Cos(1.570796f) - hit.normal.y * Mathf.Sin(1.570796f);
                    dir.y = hit.normal.x * Mathf.Sin(1.570796f) + hit.normal.y * Mathf.Cos(1.570796f);
                    dir.z = 0;

                    if (hit.collider.bounds.size.x > hit.collider.bounds.size.y)
                        lookingDistance = hit.collider.bounds.size.x * (1 + offsetFromObstacles);
                    else
                        lookingDistance = hit.collider.bounds.size.y * (1 + offsetFromObstacles);
                }
                else
                {
                    dir.x = hit.normal.x * Mathf.Cos(1.570796f) - hit.normal.z * Mathf.Sin(1.570796f);
                    dir.z = hit.normal.x * Mathf.Sin(1.570796f) + hit.normal.z * Mathf.Cos(1.570796f);
                    dir.y = 0;

                    // Choose looking distance lenght equal to max obstacle bounds extents
                    if (hit.collider.bounds.size.x > hit.collider.bounds.size.z)
                        lookingDistance = hit.collider.bounds.size.x * (1 + offsetFromObstacles);
                    else
                        lookingDistance = hit.collider.bounds.size.z * (1 + offsetFromObstacles);
                }

                if (lookingDistance>maxLookingDistance)
                    lookingDistance = maxLookingDistance;

                pos = waypoints[waypoints.Count - 1];

                // Set direction if there are more obstacles. Choose side to move
                if (Physics.Raycast(pos, targetPosition - pos, out hit, lookingDistance))
                {
                    if (Vector3.Distance(targetPosition, pos + dir * lookingDistance) > Vector3.Distance(targetPosition, pos - dir * lookingDistance))
                        dir = (pos - dir * lookingDistance) - raycastedPoint;
                }
                else // If there is no other obstacles - set direction straight to target
                    dir = targetPosition - pos;
            }
            else
            {
                if (Vector3.Distance(pos, targetPosition) < maxLookingDistance)
                {
                    // If there is no colliders - set direction straight to target and create new waypoint.
                    if (dir == (targetPosition - pos))
                        raycastedPoint = targetPosition;
                    else
                        raycastedPoint = ray.GetPoint(lookingDistance - offsetFromObstacles);

                    lookingDistance = maxLookingDistance;
                    waypoints.Add(raycastedPoint);

                    pos = waypoints[waypoints.Count - 1];
                    dir = targetPosition - pos;
                }
                else
                {
                    noPathFound = true;
                    break;
                }
            }
        }

        if (waypoints.Count >= maxComplexity)
            noPathFound = true;
    }

    public bool ShowGizmos = false;

    //----------------------------------------------------------------------------------
    // Draw debug visualization
    void OnDrawGizmos()
    {
        if (ShowGizmos == false)
            return;

        Gizmos.color = color;

        if (waypoints.Count > 0)
        {
            for (var i = 0; i < (waypoints.Count - 1); i++)
            {
                Gizmos.DrawWireSphere(waypoints[i], offsetFromObstacles / 2);
                Gizmos.DrawWireSphere(waypoints[i + 1], offsetFromObstacles / 2);

                Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
            }
        }
    }
}
//----------------------------------------------------------------------------------