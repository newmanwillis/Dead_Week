using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public Texture sprite;
	public float scale;
	public int maxHealth;
	public int health;
	
	int poisonCounter = 0;
	
	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(sprite.width*scale, 1, sprite.height*scale);
		//Debug.Log("width: " + sprite.width + " height: " + sprite.height);
		
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			Destroy (gameObject);
		}
		
		// since there are no enemies, we'll just have the player be poisoned
		poisonCounter = (poisonCounter + 1) % 300;
		if (poisonCounter == 0) {
			health--;
		}
		
		if (Input.GetButton("Jump")) {
			Debug.Log("Pressed space");
		}
	}
	
	void OnGUI() {
		//GUI.depth = 0;
		//Camera.main.GetComponent<CameraControl>().drawSpriteInWorld(sprite, transform.position.x, transform.position.y);
	}
}
