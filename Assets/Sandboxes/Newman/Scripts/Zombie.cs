using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {
	
	public enum ZombieState {wandering, chasing, attacking, dying}
	
	public float wanderSpeed = 0.25f;
	public float chaseSpeed = 0.5f;
	public int health = 100;
	
	public ZombieWander zombieWander;
	
	public ZombieState curState;
	
	// Use this for initialization
	void Start () {
		zombieWander = transform.GetComponent<ZombieWander>();
		
		curState = ZombieState.wandering;
		
	}
	
	// Update is called once per frame
	void Update () {
	
		switch(curState){
		case ZombieState.wandering:
			zombieWander.Wander();
		break;
				
			
		}
		
	}
}
