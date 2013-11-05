using UnityEngine;
using System.Collections;

public class ZombieSM : MonoBehaviour {
	
	public enum ZombieState {Wander, Chase, Attack, TakingDamage, Die, ControlledMovement, Stop};
	public ZombieState curState;
	
	private tk2dSprite sprite;
	private tk2dSpriteAnimator curAnim;
	
	// scripts on Zombie
	private ZombieWander _wander;
	private ZombieChase _chase;
	
	void Awake(){
		curState = ZombieState.Wander;
	}
	
	// Use this for initialization
	void Start () {
		sprite = GetComponent<tk2dSprite>();
		curAnim = GetComponent<tk2dSpriteAnimator>();
		
		//Debug.Log("in Zombie Start");		
		_wander = transform.GetComponent<ZombieWander>();
		_chase = transform.FindChild("ZombieDetectionRange").GetComponent<ZombieChase>();
		
		
		// Do the wander change in here, then add a "changeStateToWander" Function
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
	
	//void FixedUpdate(){
	
		//switch(curState){
		//	case ZombieState.ControlledMovement:	
			
		//}
		
	//}
	
	public void SetStateToChase(){
		if(curState != ZombieSM.ZombieState.Die && curState != ZombieSM.ZombieState.Chase){
			curState = ZombieState.Chase;
			if (_chase != null) {  // the boss zombie does not have one
				_chase.PreCalculateChase();
			}
		}
	}	
}
