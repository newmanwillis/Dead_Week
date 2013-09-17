using UnityEngine;
using System.Collections;

public class ZombieFollowPlayer : MonoBehaviour {
	
	public float moveSpeed = 0.2f;
	public float attackRange = 1.5f;
	bool foundPlayer = false;
	Transform player;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate(){
		if(foundPlayer){
			Vector3 newPos = transform.position;
			Vector3 distance = transform.position - player.transform.position;
			if(distance.x > moveSpeed)
				newPos.x -= moveSpeed;
			else if(distance.x < -moveSpeed)
				newPos.x += moveSpeed;
			if(distance.y > moveSpeed)
				newPos.y -= moveSpeed;
			else if(distance.y < -moveSpeed)
				newPos.y += moveSpeed;			
			
			transform.position = newPos;
		}
	}
	
	void OnTriggerEnter(Collider other){
		Debug.Log("In Colliderr Enter");
		print ("in collider");
		if(other.tag == "Player"){
			player = other.transform;
			foundPlayer = true;
		}
	}
}
