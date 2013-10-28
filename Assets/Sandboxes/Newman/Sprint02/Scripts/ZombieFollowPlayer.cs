using UnityEngine;
using System.Collections;

public class ZombieFollowPlayer : MonoBehaviour {
	
	public float moveSpeed = 0.2f;
	public float attackRange = 1.5f;
	bool foundPlayer = false;
	Transform player;
	
	public Player.FacingDirection curDirection;
	
	private ZombieHealth zombieHealthScript;
	
	// Use this for initialization
	void Start () {
		zombieHealthScript = transform.parent.GetComponent<ZombieHealth>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate(){
		if(foundPlayer && !zombieHealthScript.isStunned && !zombieHealthScript.isDead){
			

			
			//player = GameObject.Find("Player").transform;
			//Vector3 distance = player.position - transform.position;
			//transform.Find("ZombieSprite").LookAt(player); // enemy looks to you
			//transform.Translate(Vector3.forward * moveSpeed); // enemy walks to you
			
			
			
			Vector3 newPos = transform.position;
			
			//player = GameObject.Find("Player").transform;
			
			Vector3 move = new Vector3(0,0,0);
			
			Vector3 direction = player.transform.position - transform.position;
			direction = direction.normalized;
			curDirection = findFacingDir(direction);
			
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
	
	Player.FacingDirection findFacingDir(Vector3 direction) {
		Vector3 abs = new Vector3(0, 0, 0) + direction;  // want a copy, not a reference to the same one
		abs.x = Mathf.Abs(abs.x);
		abs.y = Mathf.Abs(abs.y);
		if (abs.y > abs.x) {
			if (direction.y > 0) {
				return Player.FacingDirection.Up;
			} else {
				return Player.FacingDirection.Down;
			}
		} else {
			if (direction.x > 0) {
				return Player.FacingDirection.Right;
			} else {
				return Player.FacingDirection.Left;
			}
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			player = other.transform;
			foundPlayer = true;
		}
	}
}
