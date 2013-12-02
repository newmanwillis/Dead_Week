using UnityEngine;
using System.Collections;

public class LaserRoomSlidingDoor : MonoBehaviour {

	private bool PowerOn = false;

	private bool open;
	private int numPeopleInRange;

	void Start () {
		open = false;
		numPeopleInRange = 0;
	}
	
	void FixedUpdate() {
		if (!animation.isPlaying) {
			Collider[] colliders = Physics.OverlapSphere(transform.position, 50);
			bool personInRange = false;
			foreach (Collider col in colliders) {
				if (col.tag == "Player" || col.tag == "Zombie") {
					personInRange = true;
					break;
				}
			}
			if (open && !personInRange) {
				// we need to close
				animation.Play("SlidingDoorClose");
				GetComponent<AudioSource>().Play();
				open = false;
			}
			if (personInRange && (!open) && PowerOn) {
				animation.Play("SlidingDoorOpen");
				GetComponent<AudioSource>().Play();
				open = true;
			}
		}
	}
	

	public void TurnPowerOn(){
		PowerOn = true;

	}

}
