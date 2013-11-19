using UnityEngine;
using System.Collections;

public class SlidingDoor : MonoBehaviour {
	private Generator generator;
	
	/***************************************************************************
	 * open and this.animation are both protected by the this.animation's lock *
	 * open is always set to whatever the state will be at the end of the      *
	 * current animation                                                       *
	 ***************************************************************************/
	private bool open;
	private int numPeopleInRange;
	
	void Start () {
		generator = GameObject.Find("Generator").GetComponent<Generator>();
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
			if (personInRange && (!open) && generator.IsRunning) {
				animation.Play("SlidingDoorOpen");
				GetComponent<AudioSource>().Play();
				open = true;
			}
		}
	}
	
	
}
