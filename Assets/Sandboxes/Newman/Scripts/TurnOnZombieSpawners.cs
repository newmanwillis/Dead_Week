using UnityEngine;
using System.Collections;

// This script is purely for performance issues. It waits to activate the last two zombie spawners
// until the player gets closerto them so there are less zombies on screen at once
public class TurnOnZombieSpawners : MonoBehaviour {

	private GameObject ZombieSpawnerNum3;
	private GameObject ZombieSpawnerNum4;
	
	void Start () {
		ZombieSpawnerNum3 = GameObject.Find("ZombieSpawner2-3");
		ZombieSpawnerNum4 = GameObject.Find("ZombieSpawner2-4");

		ZombieSpawnerNum3.SetActive(false);
		ZombieSpawnerNum4.SetActive(false);
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			ZombieSpawnerNum3.SetActive(true);
			ZombieSpawnerNum4.SetActive(true);

			// Stop checking once activated
			GetComponent<BoxCollider>().enabled = false;
		}
	}
}
