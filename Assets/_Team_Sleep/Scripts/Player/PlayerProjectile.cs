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
		print ("TAG: " + other.tag);
		if (other.tag == "Zombie" && !(dealDamageOnce && dealtDamage)) {
			//print ("in first");
			//other.GetComponent<ZombieHealth>().health -= damage;
			other.GetComponent<ZombieHealth>().TakeDamage(damage);
			dealtDamage = true;
			Destroy(gameObject);
		}
		
		if (other.tag == "Destructible" || other.tag == "Meltable") {
			Destroy(other.gameObject);
			dealtDamage = true;
		}
		
		if (other.tag == "Zombie" || other.tag == "Wall" || other.tag == "Destructible" || other.tag == "Meltable") {
			if (disappearOnCollide) {
				if (spawnOther != null) {
					Instantiate(spawnOther, transform.position, Quaternion.identity);
				}
				Destroy(gameObject);
			}	
		}
	}
}
