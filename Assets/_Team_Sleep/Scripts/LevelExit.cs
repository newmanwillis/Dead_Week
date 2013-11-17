using UnityEngine;
using System.Collections;

public class LevelExit : MonoBehaviour {
	public string nextLevelSceneName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			if (nextLevelSceneName != null && nextLevelSceneName != "") {
				PlayerPrefs.SetString(nextLevelSceneName, "Unlocked");
				PlayerPrefs.Save();
				Application.LoadLevel(nextLevelSceneName);
			}
			Destroy(this.gameObject);
		}
	}
}
