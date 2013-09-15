using UnityEngine;
using System.Collections;

public class BasicMovement : MonoBehaviour {
	
	float Speed = 5.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate(){
		Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
		direction.Normalize();
		transform.position += direction * Speed;
	}
}
