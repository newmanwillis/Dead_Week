using UnityEngine;
using System.Collections;

public class MoveZombieZone2 : MonoBehaviour {
	
	public float Direction = -1;
	public float enumMoveTime = 1f;
	
	private Vector3 contMove;
	private Vector3 enumMove;
	
	void Start () {
		contMove = new Vector3(0, 1.1f * Direction, 0);
		enumMove = new Vector3(0, 60 * Direction, 0);
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Zombie"){
			// print ("IN MOVE 2 found zombie");
			ZombieSM zsm = other.GetComponent<ZombieSM>();
			if(zsm.curState == ZombieSM.ZombieState.Stop || zsm.curState == ZombieSM.ZombieState.Wander){ 
				zsm.SetStateToControlledMovement(contMove);
			}
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.tag == "Zombie"){
			ZombieSM zsm = other.GetComponent<ZombieSM>();
			if(zsm.curState == ZombieSM.ZombieState.ControlledMovement){
				zsm.curState = ZombieSM.ZombieState.EnumeratedMovement;
				StartCoroutine(EnumeratedMove(other.transform, Time.time + enumMoveTime));	
			}
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
			//zsm.SetStateToChase();
			zsm.curState = ZombieSM.ZombieState.Wander;
		}
	}
}
