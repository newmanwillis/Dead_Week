//  To Do:
//		-Create Zombie Movement Script and reorganize
//			-8 directional movement (for wander too)
//		-Slow Down walk Animation	
//		-Random Speed (dependent on slow down amount of Animation?)


using UnityEngine;
using System.Collections;

public class ZombieWander : MonoBehaviour {
	
	public bool _isWandering	= false;								// ZombieState continuously checks this variable
	public float _wanderSpeed = 9f;
	public bool _canWander = true;										//  can make it so specific zombies do not wander
	public enum Direction {up, down, left, right};
	
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
		float waitTime = CalculateTimer(0.5f, 2, 10);  					// makes Zombies stand still for 1 to 5 seconds	
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
		float moveTime = CalculateTimer(0.2f, 5, 15);  					// makes Zombies wander for 1 to 3 seconds
		// print("Wander Time: " + (moveTime - Time.time));
		while(Time.time < moveTime){
			if(_state.curState != ZombieSM.ZombieState.Wander){  // Stops process if zombie state changes
				_isWandering = false;
				yield break;	
			}		
			Vector3 moveAmount = new Vector3(0, 0, 0);
			switch(moveDirection){	// 	Moves the Zombie
				case Direction.up:
					moveAmount.y = _wanderSpeed * Time.deltaTime;
					break;
				case Direction.down:
					moveAmount.y = -_wanderSpeed * Time.deltaTime;
					break;
				case Direction.right:
					moveAmount.x = _wanderSpeed * Time.deltaTime;
					break;
				case Direction.left:
					moveAmount.x = -_wanderSpeed * Time.deltaTime;
					break;					
			}
			GetComponent<CharacterController>().Move(moveAmount);				
			yield return null;
		}
		curAnim.StopAndResetFrame();									// stop animation
		StartCoroutine(StandStill());									// start cycle over again, keep wandering	
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
				curAnim.Play("walkingBackward");
				break;
			case Direction.down:
				curAnim.Play("walkingForward");
				break;
			case Direction.right:
				curAnim.Play("walkingRight");
				break;
			case Direction.left:
				curAnim.Play("walkingLeft");
				break;					
			}		
	}
}
