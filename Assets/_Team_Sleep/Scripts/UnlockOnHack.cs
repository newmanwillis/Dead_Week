using UnityEngine;
using System.Collections;

public class UnlockOnHack : MonoBehaviour {
	
	private HackableComputer[] computerTerminals;
	// Use this for initialization
	void Start () {
		GameObject[] computers = GameObject.FindGameObjectsWithTag("ComputerTerminal");
		computerTerminals = new HackableComputer[computers.Length];
		for (int i = 0; i < computers.Length; i++) {
			computerTerminals[i] = computers[i].GetComponent<HackableComputer>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		bool allHacked = true;
		foreach (HackableComputer comp in computerTerminals) {
			if (comp.hackSoFar < comp.timeToHackSeconds) {
				allHacked = false;
			}
		}
		
		if (allHacked) {
			Destroy(gameObject);
		}
	}
}
