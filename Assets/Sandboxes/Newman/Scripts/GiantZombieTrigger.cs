using UnityEngine;
using System.Collections;

public class GiantZombieTrigger : MonoBehaviour {

	private bool Activated = false;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Player" && !Activated){
			Activated = true;
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}
}
