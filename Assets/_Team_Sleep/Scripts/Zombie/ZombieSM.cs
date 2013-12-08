using UnityEngine;
using System.Collections;

public class ZombieSM : MonoBehaviour {
	
	public enum ZombieState {Wander, Chase, Attack, TakingDamage, Die, ControlledMovement, Stop, EnumeratedMovement, Cutscene};
	// public ZombieState StartState; // = ZombieState.Wander;
	public ZombieState curState;
	public bool Stoppable = true;
	public bool Cutscene = false;
	
	private tk2dSprite sprite;
	private tk2dSpriteAnimator curAnim;
	private CharacterController CC;
	
	// scripts on Zombie
	private ZombieWander _wander;
	private ZombieChase _chase;
	
	// For ControlledMovement state
	private Vector3 Move;
	
	void Awake(){
		if(Stoppable)
			curState = ZombieState.Stop;
		else if(Cutscene)
			curState = ZombieState.Cutscene;
		else
			curState = ZombieState.Wander;
		//curState = StartState;
	}

	void Start () {
		
		sprite = GetComponent<tk2dSprite>();
		curAnim = GetComponent<tk2dSpriteAnimator>();
		CC = GetComponent<CharacterController>();
		
		//Debug.Log("in Zombie Start");	

		if(curState != ZombieState.Cutscene){
			_wander = transform.GetComponent<ZombieWander>();
			_chase = transform.FindChild("ZombieDetectionRange").GetComponent<ZombieChase>();
		}
		// Do the wander change in here, then add a "changeStateToWander" Function

		// Start and stop animation so it doesn't glitch if interacting before animating starts
		curAnim.Play();
		curAnim.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		//print("in Zombie Update");
		switch(curState){
			case ZombieState.Wander:	// Change this so it straight calls the same method being used, dont have to check this every time.
				if(!_wander._isWandering)
					_wander.StartWanderProcess();
				break;  // Do nothing, let ZombieWander script continue			
		}
		
		
	}
	
	void FixedUpdate(){
	
		if(transform.position.z != -0.01){
			Vector3 newPos = transform.position; //new Vector3(transform.position.x, transform.position.y, 0);
			newPos.z = 0;
			transform.position = newPos;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
		
		switch(curState){
			case ZombieState.ControlledMovement:		
				CC.Move(Move);
				// print ("IN Controlled Movement");
				// ControlledMovement();
				break;		
		}		
		
	}
	/*
	public void ControlledMovement(){
		// ZombieInfo.Animate.WalkDown(curAnim);
		CC.Move(Move);
	}*/
	
	public void SetStateToControlledMovement(Vector3 move){
		Move = move;
		if(move.y < 0)
			ZombieInfo.Animate.WalkDown(curAnim);
		else
			ZombieInfo.Animate.WalkUp(curAnim);
		
		curState = ZombieState.ControlledMovement;
	}
	
	public void SetStateToChase(){
		if(curState != ZombieSM.ZombieState.Die && curState != ZombieSM.ZombieState.Chase){
			curState = ZombieState.Chase;
			if (_chase != null) {  // the boss zombie does not have one
				_chase.PreCalculateChase();
			}
		}
	}	
	/*
	void OnControllerColliderHit(ControllerColliderHit hit) {
		print ("In ControllerColliderHit");
		if(hit.gameObject.tag == "ActivateZombie"){
			print ("ColliderHit Activate");
		}
	}

	void OnCollisionEnter(Collision collision) {
		print ("In CollisionEnter");
		if(collision.gameObject.tag == "ActivateZombie"){
			print ("CollisionEnter Activate");
		}
	}

	void OnTriggerEnter(Collider other) {
		print ("In TriggerEnter");
		if(other.tag == "ActivateZombie"){
			print ("TriggerEnter Activate");
		}
	}*/


}
