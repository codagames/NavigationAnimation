using UnityEngine;

[RequireComponent (typeof (Animator))]
public class LocomotionSimpleAgent : MonoBehaviour
{
    public Vector3 destination; 

	Animator anim;
	Vector2 velocity = Vector2.zero;
    Vector2 targetVelocity = Vector2.zero; 

	void Start () {
		anim = GetComponent<Animator> ();
	    destination = transform.position; 
	}

    public float rotationSpeed = 8f;

    public enum RotationMode
    {
        None, 
        Destination, 
        Origin
    }
    public float accel = 8f;
    public float targetSpeed = 4f; 
    public RotationMode rotationMode = RotationMode.Destination;

    //void Update()
    //{
    //    if(anim)
    //    {
    //        anim.MatchTarget(destination, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(1,1,1), 1), 0f, 1f);   
    //    }
    //}

    private bool matching = false; 

	void Update ()
	{
	    Vector3 rotationTargetPos = Vector3.zero;
	    Vector3 forward = transform.TransformPoint(Vector3.forward); 
	    switch(rotationMode)
	    {
            case RotationMode.None:
	            rotationTargetPos = forward;  
	            break; 
            case RotationMode.Destination:
	            if (transform.position != destination)
	                rotationTargetPos = destination;
	            else
	                rotationTargetPos = forward; 
	            break; 
            case RotationMode.Origin:
	            rotationTargetPos = Vector3.zero; 
                break; 
	    }
        Vector3 xzPos = new Vector3(transform.position.x, 0f, transform.position.z);
	    Quaternion wantedRot = Quaternion.LookRotation(rotationTargetPos - xzPos);
	    transform.rotation = Quaternion.Slerp(transform.rotation, wantedRot, Time.unscaledDeltaTime * rotationSpeed);
	    Debug.DrawLine(transform.position, rotationTargetPos, Color.red);
        Debug.DrawLine(transform.position, forward, Color.cyan);

        Vector3 worldDeltaPosition = destination - transform.position;
        float distToDest = worldDeltaPosition.magnitude;


	    //if (transform.position == destination)
	    //{
	    //    velocity.x = 0f;
	    //    velocity.y = 0f;
	    //    matching = false; 
	    //}
     //   else if (distToDest < .2f)
     //   {
     //       velocity.x = .2f;
	    //    velocity.y = .2f;
     //       if (!matching)
     //       {
     //           anim.MatchTarget(destination, Quaternion.identity, AvatarTarget.Root,
     //               new MatchTargetWeightMask(new Vector3(1, 1, 1), 1), 0f, 1f);
     //           matching = true;
     //       }
     //   }
	    //else
     //   {
            //matching = false; 
		    // Map 'worldDeltaPosition' to local space
		    float dx = Vector3.Dot (transform.right, worldDeltaPosition);
		    float dy = Vector3.Dot (transform.forward, worldDeltaPosition);
		    Vector2 deltaPosition = new Vector2 (dx, dy);

		    // Update velocity if delta time is safe
		    //if (Time.deltaTime > 1e-5f)
	        targetVelocity = deltaPosition.normalized * targetSpeed; // Vector3.ClampMagnitude(deltaPosition / Time.deltaTime, maxSpeed);

	        velocity = Vector2.Lerp(velocity, targetVelocity, accel*Time.unscaledDeltaTime);


	        //if (velocity.magnitude*Time.deltaTime > distToDest)
	        //{
         //       Debug.Log("preventing overshoot");
	        //    velocity = Vector2.zero; //worldDeltaPosition*Time.deltaTime;
	        //}

	        bool shouldMove = //velocity.magnitude > 0.5f && 
            worldDeltaPosition.magnitude > .5f;	   
	    //}

		// Update animation parameters
	    if (shouldMove)
	    {
	        anim.SetBool("move", shouldMove);
	        anim.SetFloat("velx", velocity.x, .1f, Time.deltaTime);
	        anim.SetFloat("vely", velocity.y, .1f, Time.deltaTime);
	    }
	    else
	    {
	        anim.SetFloat("velx", 0f, .1f, Time.deltaTime);
	        anim.SetFloat("vely", 0f, .1f, Time.deltaTime);
	    }

	    //LookAt lookAt = GetComponent<LookAt> ();
		//if (lookAt)
		//	lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;

	}

    //public void MatchTarget()
    //{
    //    	    Quaternion wantedRot = Quaternion.LookRotation(destination - transform.position);

    //            anim.MatchTarget(destination, wantedRot, AvatarTarget.Root, new MatchTargetWeightMask(Vector3.one, 1f), .1f, .5f);
    //}

	void OnAnimatorMove () {
		// Update postion to agent position
//		transform.position = agent.nextPosition;

		// Update position based on animation movement using navigation surface height

	    //float distToTarget = Vector3.Magnitude(transform.position - destination); 

	    //if (distToTarget < correctionRadius)
	    //{
	    //    if (transform.position == destination)
	    //    {
     //           Debug.Log("DESTINATION REACHED!");
	    //    }
	    //    else
	    //    {
	    //        Debug.Log("correcting...");
	    //        anim.rootPosition = Vector3.Lerp(anim.rootPosition, destination, Time.unscaledDeltaTime*correctionSpeed);
	    //    }
	    //}

	    Vector3 position = anim.rootPosition;
	    position.y = 0f; 
		transform.position = position;
	}

    public float correctionRadius = 1f; 
    public float correctionSpeed = 1f; 
}
