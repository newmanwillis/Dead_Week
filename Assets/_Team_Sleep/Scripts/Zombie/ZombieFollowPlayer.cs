using UnityEngine;
using System.Collections;

public class ZombieFollowPlayer : MonoBehaviour {
	
	public float moveSpeed = 0.2f;
	public float attackRange = 1.5f;
	bool foundPlayer = false;
	Transform player;
	
	private ZombieHealth zombieHealthScript;
	
	// Use this for initialization
	void Start () {
		zombieHealthScript = transform.parent.GetComponent<ZombieHealth>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate(){
		if(foundPlayer && !zombieHealthScript.IsStunned && !zombieHealthScript.isDead){
			

			
			//player = GameObject.Find("Player").transform;
			//Vector3 distance = player.position - transform.position;
			//transform.Find("ZombieSprite").LookAt(player); // enemy looks to you
			//transform.Translate(Vector3.forward * moveSpeed); // enemy walks to you
			
			
			
			Vector3 newPos = transform.position;
			
			//player = GameObject.Find("Player").transform;
			
			Vector3 move = new Vector3(0,0,0);
			
			Vector3 distance = transform.position - player.transform.position;
			if(distance.x > moveSpeed)
				//newPos.x -= moveSpeed;
				move.x = -1 * moveSpeed;
				
			else if(distance.x < -moveSpeed)
				//newPos.x += moveSpeed;
			
				move.x = 1 * moveSpeed;
			if(distance.y > moveSpeed)
				//newPos.y -= moveSpeed;
				move.y = -1 * moveSpeed;
			else if(distance.y < -moveSpeed)
				//newPos.y += moveSpeed;			
				move.y = 1 * moveSpeed;
			
			
			//transform.position = newPos;
			transform.parent.GetComponent<CharacterController>().Move(move);
			// GetComponent<CharacterController>().Move(move);
			
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			player = other.transform;
			foundPlayer = true;
		}
	}
}
