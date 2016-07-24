// ClickToMove.cs
using UnityEngine;

[RequireComponent(typeof(LocomotionSimpleAgent))]
public class ClickToMove : MonoBehaviour
{
	RaycastHit hitInfo = new RaycastHit();
    private LocomotionSimpleAgent agent; 

    void Start()
    {
        agent = GetComponent<LocomotionSimpleAgent>(); 
    }

    void Update () {
		if(Input.GetMouseButton(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		    if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
		    {
		        agent.destination = new Vector3(hitInfo.point.x, 0f, hitInfo.point.z);
		        //agent.MatchTarget();
		    }
		}
	}
}
