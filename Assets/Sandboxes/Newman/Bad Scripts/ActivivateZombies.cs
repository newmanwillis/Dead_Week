using UnityEngine;
using System.Collections;

public class ActivivateZombies : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if(other.tag == "Zombie"){
			ZombieSM zsm = other.GetComponent<ZombieSM>();
			if(zsm.Stoppable && zsm.curState == ZombieSM.ZombieState.Stop){
				zsm.curState = ZombieSM.ZombieState.Wander;
			}
		}
	}
}
