using UnityEngine;
using System.Collections;

public class TriggeredZombieMovement : MonoBehaviour {

	public Vector3 Move;
	// private Transform LaserGateZombie;
	// ZombieSM zsm;

	// Use this for initialization
	void Start () {
		// LaserGateZombie = transform.GetChild(0);
		// zsm = LaserGateZombie.GetComponent<ZombieSM>();

		//zsm.curState = ZombieSM.ZombieState.CutScene;



	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			transform.GetChild(0).GetComponent<ZombieSM>().SetStateToControlledMovement(Move);
			// zsm.SetStateToControlledMovement(Move);	
			gameObject.collider.enabled = false;
		}
	}
}
