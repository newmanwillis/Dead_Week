// This script requires objects to be labeled with correct tags and layers
// Tags: Player, Wall
// Layers: Default, Wall, Player

// TO DO: restructure calculateChse code, change detectioncollider to change with facing direction

using UnityEngine;
using System.Collections;

public class ZombieChase : MonoBehaviour {
	
	int _speed = 52;
	public int _speed1 = 52;
	public int _speed2 = 58;
	public int _speed3 = 70;
	
	
	private float _lookForPlayerTimer = 0;	
	private float _lookForPause = 0.5f;
	private float _stopChaseTimer = 0;
	private float _stopChaseDelay = 3.0f; 
	private float _xOffset = 7f;
	private float _yOffset = 13f;


	private bool _foundPlayer = false;
	private bool _outsideDetectionRange = true;
	
	Transform Player;
	Transform Zombie;	
	private ZombieSM _state;
	private tk2dSpriteAnimator curAnim;
	private CharacterController CC;
	
	// Use this for initialization
	void Start () {
		int[] chaseSpeeds = {_speed1, _speed2, _speed3};
		int speedIndex = Random.Range(0, chaseSpeeds.Length);
		_speed = chaseSpeeds[speedIndex];
		//_speed = 72;
		//_speed = Random.Range(35, 52);
		
		// print ("SPEED: " + _speed);
		
		Zombie = transform.parent;
		_state = Zombie.GetComponent<ZombieSM>();
		curAnim = Zombie.GetComponent<tk2dSpriteAnimator>();
		CC = Zombie.GetComponent<CharacterController>();
	}
	
	void OnTriggerStay(Collider other){
		if(!_foundPlayer){	
			// Keeps checking for player every 0.5 seconds if in detection range	
			// but behind a wall. This is so it doesn't raycast every update
			if(other.tag == "Player" && !_foundPlayer && _lookForPlayerTimer < Time.time && _state.curState != ZombieSM.ZombieState.TakingDamage && _state.curState != ZombieSM.ZombieState.Die){
																												// Temp change from .Stunned
				int layerMask = ~(1 << 0);
				RaycastHit hit;
				if(Physics.Raycast(Zombie.position, other.transform.position - Zombie.position, out hit, 100, layerMask)){
					
					if(hit.transform.tag == "Wall"){
						_lookForPlayerTimer = Time.time + _lookForPause;					
					}
					else if(hit.transform.tag == "Player"){
						_foundPlayer = true;
						_outsideDetectionRange = false;
						Player = other.transform;
						_state.curState = ZombieSM.ZombieState.Chase;
						CalculateChase();					
					}			
					/*
					if(hit.transform.gameObject.layer == LayerMask.NameToLayer( "Wall" )){
						print("hit wall layer");
					}
					if(hit.transform.gameObject.layer == LayerMask.NameToLayer( "Player" )){
						print("hit player layer");
					}	*/		
				}
			}
		}
	}
	
	public void PreCalculateChase(){	// called from other functions when switching back to chase state
		if(!_foundPlayer){
			_foundPlayer = true;
			Player = GameObject.Find("Player").transform;
		}
		if(_outsideDetectionRange){
			_outsideDetectionRange = false;
		}
		CalculateChase();		
	}
	
	void CalculateChase(){
		if(_outsideDetectionRange && _stopChaseTimer < Time.time){		// Stops Chasing player after they have gone out of detection range for long enough
			_state.curState = ZombieSM.ZombieState.Wander;
			_foundPlayer = false;
			curAnim.Stop();
		}
		else{															// TO DO: CODE NEEDS ESSENTIAL RESTRUCTURING
			Vector3 difference = Player.position - Zombie.position;
			
			float furtherAxis;
			if(Mathf.Abs(difference.x) <= _xOffset && Mathf.Abs(difference.y) <= _yOffset){
				difference.x = 0;
				difference.y = 0;
				furtherAxis = 1;
			}
			else if(Mathf.Abs(difference.x) <= _xOffset){
				difference.x = 0;
				furtherAxis = Mathf.Abs(difference.y);
			}
			else if(Mathf.Abs(difference.y) <= _yOffset){
				difference.y = 0;
				furtherAxis = Mathf.Abs(difference.x);
			}		
			else{
				furtherAxis = Mathf.Max(Mathf.Abs(difference.x), Mathf.Abs(difference.y));
			}
			Vector3 direction = difference / furtherAxis;
			//ChooseDirectionAnimation(direction.x, direction.y);
			
			if(Mathf.Abs (direction.x) > Mathf.Abs(direction.y)){
				bool changeDir = false;
				
				if(Mathf.Abs(direction.y) > 0.7){
					float chance = Random.value;
					if(chance < 0.7f){
						direction.y = Mathf.RoundToInt(direction.y);
						direction.x = 0;
						changeDir = true;
						//print (direction.y);						
						if(direction.y < 0)
							ZombieInfo.Animate.WalkDown(curAnim);
						else
							ZombieInfo.Animate.WalkUp(curAnim);						
					}
				}
				else if(!changeDir){
					if(direction.x < 0)
						ZombieInfo.Animate.WalkLeft(curAnim);	
					else
						ZombieInfo.Animate.WalkRight(curAnim);	
					
					if(Mathf.Abs(direction.y) < 0.4f)
						direction.y = 0;
					}
			}
			else if(Mathf.Abs (direction.x) < Mathf.Abs(direction.y)){
				bool changeDir = false;
				if(Mathf.Abs(direction.x) > 0.7){
					float chance = Random.value;
					if(chance < 0.7f){
						direction.x = Mathf.RoundToInt(direction.x);
						// print (direction.x);
						direction.y = 0;
						changeDir = true;
						if(direction.x < 0)
							ZombieInfo.Animate.WalkLeft(curAnim);	
						else
							ZombieInfo.Animate.WalkRight(curAnim);				
					}
				}
				else if(!changeDir){
					if(direction.y < 0)
						ZombieInfo.Animate.WalkDown(curAnim);
					else
						ZombieInfo.Animate.WalkUp(curAnim);
					
					if(Mathf.Abs(direction.x) < 0.4f)
						direction.x = 0;			
				}
			}
			else{  
				//  they are equal	
			}		
				
			Vector3 move = direction * _speed;
			float chaseTime = Time.time + Random.Range(0.3f, 0.8f);
			StartCoroutine(Chase(move, chaseTime));
		}	
	}
	
	IEnumerator Chase(Vector3 move, float chaseTime){
		while(Time.time < chaseTime){ 
			
			if(_state.curState != ZombieSM.ZombieState.Chase){
				yield break;	
			}
			CC.Move(move * Time.deltaTime);			
			yield return null;
		}
		CalculateChase();
	}
	
	/*
	void ChooseDirectionAnimation(float x, float y){
		if(Mathf.Abs (x) > Mathf.Abs(y)){
			if(x < 0)
				curAnim.Play("walkingLeft");
			else
				curAnim.Play("walkingRight");			
		}
		else if(Mathf.Abs (x) < Mathf.Abs(y)){
			if(y < 0)
				curAnim.Play("walkingForward");
			else
				curAnim.Play("walkingBackward");				
		}
		else{  
			//  they are equal	
		}
	}*/
	
	void OnTriggerExit(Collider other){			// Stop following player after out of detection range for long enough
		if(other.tag == "Player"){
			_stopChaseTimer = Time.time + _stopChaseDelay;	
			_outsideDetectionRange = true;
		}
		
	}
}
