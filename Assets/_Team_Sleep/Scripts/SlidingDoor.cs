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
	
	void Start () {
		generator = GameObject.Find("Generator").GetComponent<Generator>();
		open = false;
	}
	
	void OnTriggerStay(Collider other) {
		lock (animation) {
			if ((other.tag == "Player" || other.tag == "Zombie") && !open && !animation.isPlaying && generator.IsRunning) {
				animation.Play("SlidingDoorOpen");
				open = true;
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			StartCoroutine(close());
		}
	}
	
	IEnumerator close() {
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
	}
}
