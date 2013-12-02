using UnityEngine;
using System.Collections;

public class LaserRoomTurnOnButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other){

		if(other.tag == "Attack"){
			transform.FindChild("ButtonOn").gameObject.SetActive(true);
			GameObject.Find("Door1").GetComponent<LaserRoomSlidingDoor>().TurnPowerOn();
			GameObject.Find("Door2").GetComponent<LaserRoomSlidingDoor>().TurnPowerOn();
		}
	}
}
