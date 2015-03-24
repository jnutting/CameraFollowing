using UnityEngine;
using System.Collections;

public class BoundariedFollow : MonoBehaviour {
	public Transform target;
	
	// objects that mark out the corners limiting the camera's range
	public Transform upperLeftCorner;
	public Transform lowerRightCorner;
	
	Vector3 targetOffset;

	void Start() {
		targetOffset = target.position - transform.position;
	}
	
	void Update() {
		Vector3 newPosition = target.position - targetOffset;
		
		// keep camera within bounds
		newPosition.x = Mathf.Clamp (newPosition.x, upperLeftCorner.position.x, lowerRightCorner.position.x);
		newPosition.z = Mathf.Clamp (newPosition.z, lowerRightCorner.position.z, upperLeftCorner.position.z);
		
		transform.position = newPosition;
	}
}
