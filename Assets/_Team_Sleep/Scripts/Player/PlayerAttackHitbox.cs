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
	FootballZombieHealth bossHealth = null;
	
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
		if (other.tag == "FootballZombie") {
			bossHealth = other.GetComponent<FootballZombieHealth>();
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
		if (other.tag == "FootballZombie") {
			bossHealth = null;
		}
	}
	
	public void attack(int damage, float attackStart) {
		bool hitZombie = false;
		// Debug.Log("num: " + hitbox.zombies.Keys.Count);
		ArrayList deadZombies = new ArrayList();
		foreach (Collider col in zombies.Keys) {
			if (col) {
				hitZombie = true;
				ZombieHealth zombie = col.GetComponent<ZombieHealth>();
				if (zombie.LastHitTime < attackStart) {
					zombie.TakeDamage(damage, ZombieHealth.HitTypes.sword);
				}
			} else {
				deadZombies.Add(col);  // this zombie died while in range
			}
		}
		
		foreach (Collider col in deadZombies) {
			zombies.Remove(col);
		}
		
		ArrayList deadDestructibles = new ArrayList();
		foreach (Collider col in destructibles.Keys) {
			if (col) {
				col.transform.parent.gameObject.GetComponent<Destructible>().smash();
			} else {
				deadDestructibles.Add(col);
			}
		}
		
		foreach (Collider col in deadZombies) {
			destructibles.Remove(col);
		}
		
		if (bossHealth != null && bossHealth.lastSwordHitTime < attackStart) {
			hitZombie = true;
			bossHealth.lastSwordHitTime = attackStart;
			bossHealth.takeDamage(damage);
		}

		if (hitZombie) {
			AudioSource audio = GetComponent<AudioSource>();
			if (!audio.isPlaying) {
				GetComponent<AudioSource>().Play();
			}
		}
	}
}
