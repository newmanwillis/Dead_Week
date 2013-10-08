using UnityEngine;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour {
	public bool spawnZombie;
	public Vector3 spawnPos;
	public string zombieAnimation;
	
	public Transform zombie;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			other.animation.Play("PlayerSpotsZombie");
			
			if (spawnZombie) {
				Transform spawned = (Transform)Instantiate(zombie, spawnPos, Quaternion.identity);
				spawned.animation.Play("ZombieWalksLeft");
			}
			Destroy(this.gameObject);
		}
	}
}
