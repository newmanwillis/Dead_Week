using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour {
	
	public Transform Zombie;
	public float SpawnTimer = 5f;
	public float Direction = -1;
	public float MoveTime = 5;
	public float Speed = 60;
	
	private int ColliderCount = 0;
	
	private Vector3 Move;

	// Use this for initialization
	void Start () {
		Move = new Vector3(0, Speed * Direction, 0);
		
		StartCoroutine(SpawnZombie(SpawnTimer));
	}
	
	void OnTriggerEnter(Collider other){
		if(!other.isTrigger)
			ColliderCount++;
	}
	
	void OnTriggerExit(Collider other){
		if(!other.isTrigger)
			ColliderCount--;
	}
	
	IEnumerator SpawnZombie(float timer){
		
		yield return new WaitForSeconds(SpawnTimer);
		print ("Collider Count: " + ColliderCount);
		if(ColliderCount == 0){
			Transform zombie = ((Transform) Instantiate(Zombie, transform.position, Quaternion.identity) );
			zombie.GetComponent<ZombieSM>().curState = ZombieSM.ZombieState.ControlledMovement;
			
			//choose walk animation based on what direction zombie will go
			if(Direction < 0){
				ZombieInfo.Animate.WalkDown(zombie.GetComponent<tk2dSpriteAnimator>());
				print ("DOWN ANIM CHOSEN");
			}
			else{
				
				ZombieInfo.Animate.WalkUp(zombie.GetComponent<tk2dSpriteAnimator>());
			}
			
			StartCoroutine(MoveZombie(zombie, Time.time + MoveTime));
			StartCoroutine(SpawnZombie(SpawnTimer));
		}
		else{
			StartCoroutine(SpawnZombie(1));
		}
		
	}
	
	IEnumerator MoveZombie(Transform zombie, float moveTime){
		while(Time.time < moveTime){ 
			// zombie.GetComponent<tk2dSpriteAnimator>().Play();
			if(zombie.GetComponent<ZombieSM>().curState != ZombieSM.ZombieState.ControlledMovement){		
				yield break;	

			}
			zombie.GetComponent<CharacterController>().Move(Move * Time.deltaTime);			
			yield return null;	
		}
		zombie.GetComponent<ZombieSM>().curState = ZombieSM.ZombieState.Wander;
		zombie.GetComponent<tk2dSpriteAnimator>().Stop();
	}	
}
