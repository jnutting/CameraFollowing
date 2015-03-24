using UnityEngine;
using System.Collections;

public class BasicFollow : MonoBehaviour {
	// The object we are following
	public Transform target;
	
	// A vector pointing from self to the target
	Vector3 targetOffset;

	void Start() {
		targetOffset = target.position - transform.position;
	}
	
	void Update() {
		transform.position = target.position - targetOffset;
	}
}
