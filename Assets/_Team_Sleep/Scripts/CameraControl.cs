using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	GameObject playerObject;
	PlayerScript player;
	
	public Texture heartSpriteFull;
	public Texture heartSpriteHalf;
	public Texture heartSpriteEmpty;
	
	public Texture winMessage;
	public Texture loseMessage;
	public Texture blackBox;
	public Texture blueBox;
	
	public Texture healthBar;
	
	// Use this for initialization
	void Start () {
		//camera.orthographic = true;
		//camera.orthographicSize = Screen.height/4;
		
		playerObject = GameObject.Find("Player");
		player = playerObject.GetComponent<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	// called for GRAPHICS frames and not physics timesteps. GUI methods are only allowed in here.
	void OnGUI() {
		GUI.depth = 0;
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
		
		// draw energy bar:
		int energy = playerObject.GetComponent<PlayerMovement>().PhoneCharge;
		int energyMax = playerObject.GetComponent<PlayerMovement>().phoneMaxCharge;
		float percent = ((float) energy) / energyMax;
		GUI.DrawTexture(new Rect(Screen.width - 200, 10, 150, 20), blackBox);
		GUI.DrawTexture(new Rect(Screen.width - 200 + 5, 10 + 5, (150 - (5*2))*percent, 10), blueBox);
		
		if (GameObject.FindGameObjectsWithTag("Zombie").Length == 0) {
			Rect position = new Rect(Screen.width/2-winMessage.width/2, Screen.height/2-winMessage.height/2, winMessage.width, winMessage.height);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox);
			GUI.DrawTexture(position, winMessage);
		} else if (playerHealth() <= 0) {
			Rect position = new Rect(Screen.width/2-loseMessage.width/2, Screen.height/2-loseMessage.height/2, loseMessage.width, loseMessage.height);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox);
			GUI.Label(position, loseMessage);
		};
	}
	
	public void drawSpriteInWorld(Texture sprite, float x, float y) {
		Vector2 pixels = ConvertWorldToPixels(new Vector2(x, y));
		drawSpriteAt((int)(pixels.x - sprite.width/2), (int)(pixels.y - sprite.height/2), sprite);
	}
	
	Vector2 ConvertWorldToPixels(Vector2 world) {
		float x = world.x;
		float y = world.y;
		x -= transform.position.x;
		y -= transform.position.y;
		y *= -1;
		x += Screen.width/2;
		y += Screen.height/2;
		return new Vector2(x, y);
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
