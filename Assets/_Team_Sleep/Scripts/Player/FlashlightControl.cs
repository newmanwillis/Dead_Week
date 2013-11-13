using UnityEngine;
using System.Collections;

public class FlashlightControl : MonoBehaviour {
	
	private Transform flashLight;

	private bool flashLightOn = false;
	
	private Vector3 up = new Vector3(-35, 0, 0);
	private Vector3 down = new Vector3(35, 0, 0);
	private Vector3 left = new Vector3(0, -35, 0);
	private Vector3 right = new Vector3(0, 35, 0);
	
	private Vector3 curDirection = new Vector3(35, 0, 0);
	private Vector3 goalDirection = new Vector3(35, 0, 0);
	
	public float rotationSpeed;
	
	private Light playerSpotlight;	
	private float origSpotLightRange;
	private float origSpotLightIntensity;
	
	// Use this for initialization
	void Start () {
		flashLight = transform.FindChild("FlashLight");
		flashLight.transform.Rotate(curDirection);
		flashLight.GetComponent<Light>().intensity = 0;
		
		playerSpotlight = transform.FindChild("Spotlight").GetComponent<Light>();
		origSpotLightRange = playerSpotlight.GetComponent<Light>().range;
		origSpotLightIntensity = playerSpotlight.GetComponent<Light>().intensity;
	}
	
	void FixedUpdate () {
		flashLight.transform.localRotation = Quaternion.identity;
		curDirection = Vector3.RotateTowards(curDirection, goalDirection, rotationSpeed * Time.deltaTime, 0);
		flashLight.transform.Rotate(curDirection);
	}
	
	public void face(Player.FacingDirection dir) {
		switch (dir) {
		case Player.FacingDirection.Up:
			goalDirection = up;
			break;
		case Player.FacingDirection.Down:
			goalDirection = down;
			break;
		case Player.FacingDirection.Left:
			goalDirection = left;
			break;
		case Player.FacingDirection.Right:
			goalDirection = right;
			break;
		}
	}
	
	public void toggleOnOff() {
		if (flashLightOn) {
			flashLightOn = false;
			flashLight.GetComponent<Light>().intensity = 0;
			playerSpotlight.range = origSpotLightRange;
			playerSpotlight.intensity = origSpotLightIntensity;			
			
		} else {
			flashLightOn = true;
			flashLight.GetComponent<Light>().intensity = 6;
			playerSpotlight.range = 110;
			playerSpotlight.intensity = 0.5f;
		}
	}
}
