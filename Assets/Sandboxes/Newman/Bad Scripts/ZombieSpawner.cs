using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour {
	
	public Transform Zombie;
	private float SpawnTimer = 5f;
	//public float Direction = -1;
	private float MoveTime = 5;
	//public float Speed = 60;
	
	private float Direction;
	private float Speed;
	
	private Transform MoveZone;
	
	private int ColliderCount = 0;
	
	private Vector3 Move;

	// Use this for initialization
	void Start () {
		SpawnerAttributes sa = transform.parent.GetComponent<SpawnerAttributes>();
		Speed = sa.Speed;
		Direction = sa.Direction;
		SpawnTimer = sa.SpawnTimer;
		MoveTime = sa.MoveTime;
		
		MoveZone = transform.parent.parent.FindChild("MoveZone");
		
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
		if(ColliderCount == 0){
			Transform zombie = ((Transform) Instantiate(Zombie, transform.position, Quaternion.identity) );
			zombie.GetComponent<ZombieSM>().curState = ZombieSM.ZombieState.ControlledMovement;
			
			//choose walk animation based on what direction zombie will go
			if(Direction < 0){
				ZombieInfo.Animate.WalkDown(zombie.GetComponent<tk2dSpriteAnimator>());
			}
			else{
				
				ZombieInfo.Animate.WalkUp(zombie.GetComponent<tk2dSpriteAnimator>());
			}
			
			StartCoroutine(MoveZombie(zombie, Time.time + MoveTime));
			StartCoroutine(SpawnZombie(SpawnTimer));
		}
		else{
			StartCoroutine(SpawnZombie(1.2f));
		}
		
	}
	
	IEnumerator MoveZombie(Transform zombie, float moveTime){
		ZombieSM zsm = zombie.GetComponent<ZombieSM>();
		CharacterController CC = zombie.GetComponent<CharacterController>();
		while(Time.time < moveTime){ 
			// zombie.GetComponent<tk2dSpriteAnimator>().Play();
			if(zsm.curState != ZombieSM.ZombieState.ControlledMovement){		
				yield break;	

			}
			CC.Move(Move * Time.deltaTime);			
			yield return null;	
		}
		// check if in movezone, then enumerate until out. // make the move zone into collider for just zombies
		if(MoveZone.collider.bounds.Contains(zombie.position) && zsm.curState == ZombieSM.ZombieState.ControlledMovement){
			StartCoroutine(MoveZombie(zombie, Time.time + 1));			
		}
		else{
			zsm.curState = ZombieSM.ZombieState.Wander;
			zombie.GetComponent<tk2dSpriteAnimator>().Stop();
		}
	}	
}
