using UnityEngine;
using System.Collections;

public class MoveZombieZone2 : MonoBehaviour {
	
	public float Direction = -1;
	
	private Vector3 contMove;
	private Vector3 enumMove;

	
	// Use this for initialization
	void Start () {
		contMove = new Vector3(0, 1.1f * Direction, 0);
		enumMove = new Vector3(0, 60 * Direction, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		// print ("IN MOVE 2");
		if(other.tag == "Zombie"){
			// print ("IN MOVE 2 found zombie");
			ZombieSM zsm = other.GetComponent<ZombieSM>();
			if(zsm.curState == ZombieSM.ZombieState.Wander){
			//other.GetComponent<ZombieSM>().curState	= ZombieSM.ZombieState.ControlledMovement;
				// print ("IN MOVE 2 WANDER");
				//zsm.curState = ZombieSM.ZombieState.ControlledMovement;	
				zsm.SetStateToControlledMovement(contMove);
			}
		}
	}
	
	void OnTriggerExit(Collider other){
		
		if(other.tag == "Zombie"){
			other.GetComponent<ZombieSM>().curState = ZombieSM.ZombieState.EnumeratedMovement;
			StartCoroutine(EnumeratedMove(other.transform, Time.time + 1.2f));	
			
		}
		
	}
	
	IEnumerator EnumeratedMove(Transform zombie, float moveTime){
		CharacterController CC = zombie.GetComponent<CharacterController>();
		ZombieSM zsm = zombie.GetComponent<ZombieSM>();
		
		while(Time.time < moveTime){
			if(zsm.curState != ZombieSM.ZombieState.EnumeratedMovement){		
				yield break;
			}
			CC.Move(enumMove * Time.deltaTime);
				
			yield return null;	
		}
		if(zsm.curState == ZombieSM.ZombieState.EnumeratedMovement){
			zsm.curState = ZombieSM.ZombieState.Wander;	
		}
	}
}
