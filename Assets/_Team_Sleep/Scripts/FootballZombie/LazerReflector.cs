using UnityEngine;
using System.Collections;

public class LazerReflector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Attack") {
			Vector3 inDirection = other.rigidbody.velocity;
			Vector3 reflectAxis = transform.position - other.transform.position;
			float dot = Vector3.Dot(reflectAxis, inDirection);
			Vector3 outDirection = inDirection - 2*(dot * reflectAxis);
			other.transform.position = outDirection;
			
			Debug.Log("In : " + inDirection + "\nReflect: " + reflectAxis + "\nOut: " + outDirection);
		}
	}
}
