﻿using UnityEngine;
using System.Collections;

public class ZombieSM : MonoBehaviour {
	
	public enum ZombieState {Wander, Chase, Attack, Die};
	//public static State accessState
	public ZombieState curState;
	
	public float wanderSpeed = 0.25f;
	public float chaseSpeed = 0.5f;
	public int health = 100;
	
	// scripts on Zombie
	private ZombieWander _wander;
	
	

	
	// Use this for initialization
	void Start () {
		//Debug.Log("in Zombie Start");		
		_wander = transform.GetComponent<ZombieWander>();
		
		curState = ZombieState.Wander;
		
	}
	
	// Update is called once per frame
	void Update () {
		//print("in Zombie Update");
		switch(curState){
			case ZombieState.Wander:
				if(!_wander._isWandering)
					_wander.StartWanderProcess();
				break;  // Do nothing, let ZombieWander script continue
			
				
			
		}
		
	}
}