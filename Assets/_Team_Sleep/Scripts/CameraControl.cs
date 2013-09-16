using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	GameObject playerObject;
	PlayerScript player;
	
	public Texture heartSpriteFull;
	public Texture heartSpriteHalf;
	public Texture heartSpriteEmpty;
	
	// Use this for initialization
	void Start () {
		camera.orthographic = true;
		camera.orthographicSize = Screen.height/2;
		
		playerObject = GameObject.Find("Player");
		player = playerObject.GetComponent<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	// called for GRAPHICS frames and not physics timesteps. GUI methods are only allowed in here.
	void OnGUI() {
		int health = playerHealth();
		for (int heart = 1; heart <= player.maxHealth/2; heart++) {
			int x = 10 + 50*heart;
			int y = 10;
			if (health >= 2*heart) {
				drawSpriteAt(x, y, heartSpriteFull);
			} else if (health == 2*heart - 1) {
				drawSpriteAt(x, y, heartSpriteHalf);
			} else {
				drawSpriteAt(x, y, heartSpriteEmpty);
			}
		}
	}
	
	/*
	 * x and y are in pixels indexed from the top left of the screen. The top left
	 * corner of the sprite will be at (x,y).
	 * This is only usable from OnGUI.
	 */
	void drawSpriteAt(int x, int y, Texture sprite) {
		GUI.DrawTexture(new Rect(x, y, sprite.width, sprite.height), sprite);
	}
	
	int playerHealth() {
		if (playerObject == null) { // this is apparently overloaded to check whether it has been destroyed
			return -1;
		}
		return player.health;
	}
}
