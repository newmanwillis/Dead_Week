using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	GameObject playerObject;
	PlayerScript player;
	
	public float scale;
	public int maxHealth;
	public int health;	
	
	public Texture heartSpriteFull;
	public Texture heartSpriteHalf;
	public Texture heartSpriteEmpty;
	
	// Use this for initialization
	void Start () {
		//camera.orthographic = true;
		//camera.orthographicSize = Screen.height/2;
		
		//playerObject = GameObject.Find("Player");
		//player = playerObject.GetComponent<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			Destroy (gameObject);
		}		
	}
	
	// called for GRAPHICS frames and not physics timesteps. GUI methods are only allowed in here.
	void OnGUI() {
		GUI.depth = 0;
		int health = playerHealth();
		for (int heart = 1; heart <= maxHealth/2; heart++) {
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
		//if (playerObject == null) { // this is apparently overloaded to check whether it has been destroyed
		//	return -1;
		//}
		return health;
	}
}
