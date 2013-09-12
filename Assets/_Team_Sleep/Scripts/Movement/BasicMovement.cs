using UnityEngine;
using System.Collections;

public class BasicMovement : MonoBehaviour {
	
	float Speed = 0.5f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate(){
		
		Vector3 curPos = transform.position;
		if(Input.GetKey(KeyCode.W)){
			curPos.y += Speed;
		}		
		if(Input.GetKey(KeyCode.S)){
			curPos.y -= Speed;
		}
		if(Input.GetKey(KeyCode.D)){
			curPos.x += Speed;
		}		
		if(Input.GetKey(KeyCode.A)){
			curPos.x -= Speed;
		}			
		transform.position = curPos;
	}
}
