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
				open = false;
			}
			if (personInRange && !open) {
				animation.Play("SlidingDoorOpen");
				open = true;
			}
			/*if (numPeopleInRange > 0 && !open) {
				// we need to open
				animation.Play("SlidingDoorOpen");
				open = true;
			}
			if (numPeopleInRange == 0 && open) {
				// we need to close
				animation.Play("SlidingDoorClose");
				open = false;
			}*/
		}
	}
	
	/*void OnTriggerStay(Collider other) {
		lock (animation) {
			if ((other.tag == "Player" || other.tag == "Zombie") && !open && !animation.isPlaying && generator.IsRunning) {
				animation.Play("SlidingDoorOpen");
				open = true;
			}
		}
	}*/
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" || other.tag == "Zombie") {
			//StartCoroutine(close());
			++numPeopleInRange;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "Player" || other.tag == "Zombie") {
			//StartCoroutine(close());
			--numPeopleInRange;
		}
	}
	
	/*IEnumerator close() {
		bool doneClosing = false;
		while (!doneClosing) {
			doneClosing = atomicallyAttemptToClose();
			yield return null;
		}
	}
	
	
	
	// Returns true iff the close attempt was sucessful
	private bool atomicallyAttemptToClose() {
		lock (animation) {
			if (open == false) {
				return true;
			} else {
				if (animation.isPlaying) {
					return false;
				} else {
					this.animation.Play("SlidingDoorClose");
					open = false;
					return true;
				}
			}
		}
	}*/
}
