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
		if (other.tag == "Zombie" && !(dealDamageOnce && dealtDamage)) {
			//print ("in first");
			//other.GetComponent<ZombieHealth>().health -= damage;
			if(damage > 0)
				other.GetComponent<ZombieHealth>().TakeDamage(damage, ZombieHealth.HitTypes.burstLaser);
			dealtDamage = true;
		}
		
		if (other.tag == "Destructible") {
			other.GetComponent<Destructible>().disintigrate();
			dealtDamage = true;
		}
		
		if (other.tag == "Button") {
			Debug.Log("hit button");
			other.GetComponent<Button>().power();
			//foreach (Transform possibleDoor in other.transform) {
			//	Debug.Log("door " + possibleDoor.tag);
			//	if (possibleDoor.tag == "Door") {
			//		Destroy(possibleDoor.gameObject);
			//	}
			//}
		}
		
		if (other.tag == "Zombie" || other.tag == "Wall" || other.tag == "Destructible" || other.tag == "Door" || other.tag == "Button") {
			if (disappearOnCollide) {
				if (spawnOther != null) {
					Instantiate(spawnOther, transform.position, Quaternion.identity);
				}
				Destroy(gameObject);
			}	
		}
	}
}
