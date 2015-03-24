using UnityEngine;
using System.Collections;

public class SeeMoreFollow : MonoBehaviour {

	public Transform target;
	public Transform upperLeftCorner;
	public Transform lowerRightCorner;
	Vector3 targetOffset;
	
	public float smoothing = 8.0f;
	public float velocityMultiplier = 0.0238f;
	public float cameraShiftMultiplier = 1.5f;
	
	CharacterController characterController;
	
	Vector3 originalTargetRotation;
	float xAngle;
	
	// We need the camera to change its field of view
	Camera camera;
	// Define "normal" field of view for the camera
	public float baseFieldOfView = 26;
	// Determine how much we should change the FOV in response to player velocity
	public float fieldOfViewVelocityMultiplier = 1;
	
	void Start() {
		targetOffset = target.position - transform.position;
		characterController = target.GetComponent<CharacterController>();
		originalTargetRotation = target.rotation * Vector3.forward;
		xAngle = transform.rotation.eulerAngles.x;
		camera = GetComponent<Camera>();
	}
	
	void Update() {
		float timeTickDiff = Time.deltaTime;
		float t = timeTickDiff * smoothing;
		
		Vector3 newPosition = target.position - targetOffset;
		
		Vector3 forwardVelocity = characterController.velocity;
		Quaternion movementDirection;
		if (forwardVelocity.magnitude < 0.001f) {
		    movementDirection = Quaternion.identity;
		} else {
			movementDirection = Quaternion.LookRotation(forwardVelocity);
		}
		Vector3 cameraDirection = Vector3.Lerp (originalTargetRotation, movementDirection * Vector3.forward * cameraShiftMultiplier, t);
		Quaternion newRotation = Quaternion.LookRotation(cameraDirection);
		
		Vector3 euler = newRotation.eulerAngles;
		euler.x = xAngle;
		euler.z = 0;
		newRotation = Quaternion.Euler(euler);
		
		// adjust field of view according to velocity
        camera.fieldOfView = baseFieldOfView + forwardVelocity.magnitude * fieldOfViewVelocityMultiplier;
		
		newPosition.x = Mathf.Clamp (newPosition.x, upperLeftCorner.position.x, lowerRightCorner.position.x);
		newPosition.z = Mathf.Clamp (newPosition.z, lowerRightCorner.position.z, upperLeftCorner.position.z);
		
		transform.position = Vector3.Lerp(transform.position, newPosition, t);
		transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, t);
	}
}
