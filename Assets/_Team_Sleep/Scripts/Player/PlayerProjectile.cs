using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour {
	public int damage;
	public int energyCost;
	public bool disappearOnCollide;
	public bool dealDamageOnce;
	public Transform spawnOther;
	bool dealtDamage = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider other) {
		// print ("TAG: " + other.tag);
		if ((other.tag == "Zombie" || other.tag == "FootballZombie") && !(dealDamageOnce && dealtDamage)) {
			//print ("in first");
			//other.GetComponent<ZombieHealth>().health -= damage;
			if(damage > 0) {
				if (other.tag == "Zombie") {
					other.GetComponent<ZombieHealth>().TakeDamage(damage, ZombieHealth.HitTypes.burstLaser);
				} else {
					other.GetComponent<FootballZombieHealth>().takeDamage(damage);
				}
			}
			dealtDamage = true;
		}
		
		if (other.tag == "Destructible") {
			Destructible destruct = other.GetComponent<Destructible>();
			if (destruct == null) {
				destruct = other.transform.parent.gameObject.GetComponent<Destructible>();
			}
			destruct.disintigrate();
			dealtDamage = true;
		}
		
		if (other.tag == "Button") {
			Debug.Log("hit button");
			other.GetComponent<Button>().power();
		}
		
		string[] barrierTags = { "FootballZombie", "Zombie", "Wall", "Destructible", "Door", "Button", "ElectronicDoor2" };
		//if (Array.IndexOf(barrierTags, other.tag) != -1) {
		if (System.Array.Exists(barrierTags, other.tag.Equals)) {
			if (disappearOnCollide) {
				if (spawnOther != null) {
					Instantiate(spawnOther, transform.position, Quaternion.identity);
				}
				Destroy(gameObject);
			}	
		}
	}
}
