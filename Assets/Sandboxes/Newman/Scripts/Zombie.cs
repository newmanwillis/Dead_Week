using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {
	
	public enum ZombieState {calculateWander, wandering, chasing, attacking, dying}
	//public static ZombieState accessZombieState
	public ZombieState curState;
	
	public float wanderSpeed = 0.25f;
	public float chaseSpeed = 0.5f;
	public int health = 100;
	
	// scripts on Zombie
	public ZombieWander zombieWander;
	
	

	
	// Use this for initialization
	void Start () {
		//Debug.Log("in Zombie Start");		
		zombieWander = transform.GetComponent<ZombieWander>();
		
		curState = ZombieState.calculateWander;
		
	}
	
	// Update is called once per frame
	void Update () {
		//print("in Zombie Update");
		switch(curState){
			case ZombieState.calculateWander:
				zombieWander.StartWanderProcess();
				break;
			case ZombieState.wandering:
				break;  // Do nothing, let ZombieWander script continue
			
				
			
		}
		
	}
}
