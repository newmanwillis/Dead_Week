using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
	private Light ceilingLights;
	
	public float flickerTime;
	public int numFlickers;
	
	public bool IsRunning {get; private set;}

	// Use this for initialization
	void Start () {
		ceilingLights = GameObject.Find("CeilingLights").GetComponent<Light>();
		IsRunning = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void shutdown() {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ElectronicDoor")) {
			obj.animation.Play("ElectronicDoorOpen");
		}
		StartCoroutine(flickerLights());
		IsRunning = false;
	}
	
	IEnumerator flickerLights() {
		float startTime = Time.time;
		ceilingLights.intensity = 0;
		float lightsOn = startTime;
		for (int i = 0; i < numFlickers; i++) {
			lightsOn += flickerTime;
			while (Time.time < lightsOn) {
				yield return null;
			}
			ceilingLights.intensity = 1;
			while (Time.time < lightsOn + flickerTime) {
				yield return null;
			}
			lightsOn += flickerTime;
			ceilingLights.intensity = 0;
		}
	}
}
