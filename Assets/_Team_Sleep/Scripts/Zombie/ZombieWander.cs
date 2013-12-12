//  To Do:
//		-Create Zombie Movement Script and reorganize
//			-8 directional movement (for wander too)
//		-Slow Down walk Animation	
//		-Random Speed (dependent on slow down amount of Animation?)


using UnityEngine;
using System.Collections;

public class ZombieWander : MonoBehaviour {
	
	public enum Direction {up, down, left, right};
	public AudioClip[] Sounds;

	public bool _isWandering	= false;								// ZombieState continuously checks this variable
	// public float _wanderSpeed = 15f;
	public float[] wanderSpeeds = {10f, 20f, 26f};
	public bool _canWander = true;										//  can make it so specific zombies do not wander
	
	
	private ZombieSM _state;
	private tk2dSpriteAnimator curAnim;  								// current animation for zombie	
	
	// Use this for initialization
	void Start () {

		_state = GetComponent<ZombieSM>();
		//curAnim = transform.FindChild("ZombieSprite").GetComponent<tk2dSpriteAnimator>();
		curAnim = GetComponent<tk2dSpriteAnimator>();
		
	}

	public void StartWanderProcess(){		
		//_state.curState = Zombie.ZombieState.Wander;
		if(!_isWandering && _canWander)
			_isWandering = true;
			StartCoroutine(StandStill());
	}	
	
	IEnumerator StandStill(){
		float waitTime = CalculateTimer(0.5f, 1, 4);  					// makes Zombies stand still for 0.5f to 2 seconds	
		// print("Stand Still Time: " + (waitTime - Time.time));
		while(Time.time < waitTime){
			if(_state.curState != ZombieSM.ZombieState.Wander){  // Stops process if zombie state changes
				_isWandering = false;
				yield break;	
			}		
			yield return null;
		}
		StartCoroutine(Wander());
	}
	
	IEnumerator Wander(){
		Direction moveDirection = CalculateDirection();	
		ChooseDirectionAnimation(moveDirection);
		float moveTime = CalculateTimer(0.2f, 5, 20);  					// makes Zombies wander for 1 to 4 seconds
		// print("Wander Time: " + (moveTime - Time.time));
		float wanderSpeed = ChooseWanderSpeed();
		PlayRandomSound();
		while(Time.time < moveTime){
			if(_state.curState != ZombieSM.ZombieState.Wander){  // Stops process if zombie state changes
				_isWandering = false;
				yield break;	
			}		
			Vector3 moveAmount = new Vector3(0, 0, 0);
			switch(moveDirection){	// 	Moves the Zombie
				case Direction.up:
					moveAmount.y = wanderSpeed * Time.deltaTime;
					break;
				case Direction.down:
					moveAmount.y = -wanderSpeed * Time.deltaTime;
					break;
				case Direction.right:
					moveAmount.x = wanderSpeed * Time.deltaTime;
					break;
				case Direction.left:
					moveAmount.x = -wanderSpeed * Time.deltaTime;
					break;					
			}
			// print ("HERE");
			GetComponent<CharacterController>().Move(moveAmount);				
			yield return null;
		}
		curAnim.StopAndResetFrame();									// stop animation
		StartCoroutine(StandStill());									// start cycle over again, keep wandering	
	}

	private void PlayRandomSound(){


		if(Random.value < 0.3f){
			int rand = Random.Range(0, Sounds.Length);
			//Sounds[rand]
			audio.clip = Sounds[rand];
			audio.Play();
		}
	}

	private float ChooseWanderSpeed(){
		int randSpeedIndex = Random.Range(0, wanderSpeeds.Length);
		float randSpeed = wanderSpeeds[randSpeedIndex];
		return randSpeed;
	}
	
	private float CalculateTimer(float multiplier, int min, int max){
		float timer = Time.time + (multiplier * Random.Range(min, max));
		return timer;
	}

	private Direction CalculateDirection(){
		int directionEnumLength = System.Enum.GetNames(typeof( Direction)).Length;
		int randomDirection = Random.Range(0, directionEnumLength);
		Direction randDir = (Direction) randomDirection;
		return randDir;
	}		
	
	private void ChooseDirectionAnimation(Direction moveDirection){		// Choose animation based on direction zombie will wander 
		curAnim.Resume();		
		switch(moveDirection){
			case Direction.up:
				ZombieInfo.Animate.WalkUp(curAnim);
				break;
			case Direction.down:
				ZombieInfo.Animate.WalkDown(curAnim);
				break;
			case Direction.right:
				ZombieInfo.Animate.WalkRight(curAnim);
				break;
			case Direction.left:
				ZombieInfo.Animate.WalkLeft(curAnim);
				break;					
			}		
	}
}
