using UnityEngine;
using System.Collections;

public class ZombieSM : MonoBehaviour {
	
	public enum ZombieState {Wander, Chase, Attack, TakingDamage, Die};
	//public static State accessState
	public ZombieState curState;
	
	public float wanderSpeed = 0.25f;
	public float chaseSpeed = 0.5f;
	public int health = 100;
	
	// scripts on Zombie
	private ZombieWander _wander;
	private ZombieChase _chase;
	
	

	
	// Use this for initialization
	void Start () {
		//Debug.Log("in Zombie Start");		
		_wander = transform.GetComponent<ZombieWander>();
		_chase = transform.FindChild("ZombieDetectionRange").GetComponent<ZombieChase>();
		
		curState = ZombieState.Wander;
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
			case ZombieState.Chase:
				break;
			case ZombieState.Attack:
				break;			
			// case ZombieState.TakingDamage:
			
		}
		
	}
	
	public void SetStateToChase(){
		curState = ZombieState.Chase;
		_chase.PreCalculateChase();
	}
	
	
	
}
