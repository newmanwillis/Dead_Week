using UnityEngine;
using System.Collections;

public class BasicMovement : MonoBehaviour {
	
	float Speed = 5.0f;
	
	GroundControl ground;
	
	// Use this for initialization
	void Start () {
		ground = GameObject.Find("Player").GetComponent<GroundControl>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate(){
		Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
		direction.Normalize();
		Vector3 newPos = transform.position + direction * Speed;
		if (ground.pointPathable(newPos)) {
			transform.position = newPos;
		} else {
			Debug.Log("Unpathable");
		}
	}
}
