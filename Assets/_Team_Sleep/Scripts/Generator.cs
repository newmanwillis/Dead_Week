using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
	private Light ceilingLights;
	private float lightIntensity;
	
	public float flickerTime;
	public int numFlickers;
	
	private bool isRunning;
	public bool beginsAlreadyRunning;
	public bool IsRunning {
		get {
			return isRunning;
		}
		set {
			isRunning = value;
			if (GetComponent<RepeatedlySpawn>() != null) {
				GetComponent<RepeatedlySpawn>().spawning = isRunning;
			}
			if (isRunning != beginsAlreadyRunning) {
				GameObject.Find("Player").GetComponent<Player>().setCheckpoint(transform.position + new Vector3(0, -20, 0));
			}
			if (isRunning) {
				ceilingLights.intensity = lightIntensity;
				activateAllButtons();
			} else {
				ceilingLights.intensity = 0;
			}
		}
	}
	
	public Vector3[] spawnZombieLocations;
	public Transform zombie;

	// Use this for initialization
	void Start () {
		ceilingLights = GameObject.Find("CeilingLights").GetComponent<Light>();
		lightIntensity = ceilingLights.intensity;
		if (!IsRunning) {
			ceilingLights.intensity = 0;
		}
		IsRunning = beginsAlreadyRunning;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void activateAllButtons() {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Button")) {
			obj.GetComponent<Button>().powerForever();
		}
	}
	
	public void shutdown() {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ElectronicDoorLeft")) {
			Debug.Log("opening left");
			obj.animation.Play("ElectronicDoorOpenLeft");
		}
		
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ElectronicDoorRight")) {
			Debug.Log("opening right");
			obj.animation.Play("ElectronicDoorOpenRight");
		}
		
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("DestroyOnGeneratorShutdown")) {
			Destroy(obj);
		}
		
		foreach (Vector3 spawnLoc in spawnZombieLocations) {
			Instantiate(zombie, spawnLoc, Quaternion.identity);
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
