using UnityEngine;
using System.Collections;

public class ZombieWander : MonoBehaviour {
	
	public enum wanderState {calculateWaitTime, waiting, wandering}
	public wanderState curState;
	
	private bool calculatedWaitTime = false;
	private float waitTime;
	
	// Use this for initialization
	void Start () {
		curState = wanderState.calculateWaitTime;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Wander (){
		
		switch(curState){
		
		case wanderState.calculateWaitTime:
			calculateWaitTime();
			break;
		case wanderState.waiting:
			
			break;
			
		}
		
		
		if(!calculatedWaitTime){
				
		}
	}
	
	void calculateWaitTime(){
		waitTime = 0.25f * Random.Range(2, 14);
		curState = wanderState.waiting;
	}
	
	IEnumerator waiting() {
		yield return new WaitForSeconds(waitTime);
	}
	
}
