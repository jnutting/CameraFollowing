using UnityEngine;
using System.Collections;

public class ForwardViewFollow : MonoBehaviour {

	public Transform target;
	public Transform upperLeftCorner;
	public Transform lowerRightCorner;
	
	Vector3 targetOffset;
	
	// constant adjustment for time step
	public float smoothing = 8.0f;
	// max velocity that will consider direction
	public float velocityMultiplier = 0.0238f;
	// how far should we shift the camera toward where the player's looking 
	public float cameraShiftMultiplier = 1.5f;
	
	// we need this to see which direction the character is moving
	CharacterController characterController;
	// keep the target's original forward rotation
	Vector3 originalTargetRotation;
	// keep the camera's x-axis angle constant
	float xAngle;

	void Start() {
		targetOffset = target.position - transform.position;
		characterController = target.GetComponent<CharacterController>();
		originalTargetRotation = target.rotation * Vector3.forward;
		xAngle = transform.rotation.eulerAngles.x;
	}
	
	void Update() {
		float timeTickDiff = Time.deltaTime;
		float t = timeTickDiff * smoothing;
		
		Vector3 newPosition = target.position - targetOffset;
		
		//
		// shift camera based on character's velocity and the direction the target is pointing.
		//
		
		// The character's movement velocity
		Vector3 forwardVelocity = characterController.velocity;

		// determine direction and amount toward which we should shift the camera
		Quaternion movementDirection;
		if (forwardVelocity.magnitude < 0.001f) {
		    movementDirection = Quaternion.identity;
		} else {
			movementDirection = Quaternion.LookRotation(forwardVelocity);
		}
		Vector3 cameraDirection = Vector3.Lerp (originalTargetRotation, movementDirection * Vector3.forward * cameraShiftMultiplier, t);
		
		// Make camera point at the correct spot
		Quaternion newRotation = Quaternion.LookRotation(cameraDirection);
		
		// Some of these angles we want to leave alone
		Vector3 euler = newRotation.eulerAngles;
		euler.x = xAngle;
		euler.z = 0;
		newRotation = Quaternion.Euler(euler);
		
		newPosition.x = Mathf.Clamp (newPosition.x, upperLeftCorner.position.x, lowerRightCorner.position.x);
		newPosition.z = Mathf.Clamp (newPosition.z, lowerRightCorner.position.z, upperLeftCorner.position.z);
		
		// smooth transition towards calculated camera placement
		transform.position = Vector3.Lerp(transform.position, newPosition, t);
		transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, t);
	}
}
