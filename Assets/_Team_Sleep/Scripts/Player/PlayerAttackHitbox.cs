using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttackHitbox : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public Dictionary<Collider, int> zombies = new Dictionary<Collider, int>();
	public Dictionary<Collider, int> destructibles = new Dictionary<Collider, int>();
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Zombie"){
			if (zombies.ContainsKey(other)) {
				zombies[other] += 1;
			} else {
				zombies[other] = 1;
			}
		}
		if(other.tag == "Destructible"){
			if (destructibles.ContainsKey(other)) {
				destructibles[other] += 1;
			} else {
				destructibles[other] = 1;
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		if(other.tag == "Zombie"){
			if (zombies.ContainsKey(other)) {
				zombies[other] -= 1;
				if (zombies[other] == 0) {
					zombies.Remove(other);
				}
			} else {
				Debug.LogError("removed a nonexistant zombie");
			}
		}
		if(other.tag == "Destructible"){
			if (destructibles.ContainsKey(other)) {
				destructibles[other] -= 1;
				if (destructibles[other] == 0) {
					destructibles.Remove(other);
				}
			} else {
				Debug.LogError("removed a nonexistant destructible");
			}
		}
	}
}
