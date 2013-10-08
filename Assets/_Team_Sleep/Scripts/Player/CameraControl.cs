using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	GameObject playerObject;
	Player player;
	
	public Texture heartSpriteFull;
	public Texture heartSpriteHalf;
	public Texture heartSpriteEmpty;
	
	public Texture winMessage;
	public Texture loseMessage;
	public Texture blackBox;
	public Texture blueBox;
	public Texture textMessageBox;
	
	public Texture healthBar;
	
	private bool isPaused;
	private string currentMessage;
	
	private HackableComputer[] computerTerminals;
	
	public bool isCutscene;
	
	// Use this for initialization
	void Start () {
		//camera.orthographic = true;
		//camera.orthographicSize = Screen.height/4;
		if (!isCutscene) {
			playerObject = GameObject.Find("Player");
			player = playerObject.GetComponent<Player>();
		}
		
		isPaused = false;
		
		GameObject[] computers = GameObject.FindGameObjectsWithTag("ComputerTerminal");
		computerTerminals = new HackableComputer[computers.Length];
		for (int i = 0; i < computers.Length; i++) {
			computerTerminals[i] = computers[i].GetComponent<HackableComputer>();
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	// called for GRAPHICS frames and not physics timesteps. GUI methods are only allowed in here.
	void OnGUI() {
		if (Event.current.isKey && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space) {
			if (isPaused) {
				isPaused = false;
				Time.timeScale = 1;
			} else {
				isPaused = true;
				currentMessage = "You paused the game!";
				Time.timeScale = 0;
			}
		}
		
		if (isPaused) {
			drawTextMessage(currentMessage);
		}
		
		if (!isCutscene) {
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
		
			drawEnergyBar();
		
			// draw hack bar:
			HackableComputer hacking = findCurrentHack();
			if (hacking != null) {
				float percent = hacking.hackSoFar / hacking.timeToHackSeconds;
				// 200 wide, centered
				// 40 high, starting 3/4 down the screen
				drawPercentBar(Screen.width/2 - 100, (int)(Screen.height * (3.0/4.0)), 200, 40, percent, "Hacking...");
			}
		
			if (GameObject.FindGameObjectsWithTag("Zombie").Length == 0) {
				Rect position = new Rect(Screen.width/2-winMessage.width/2, Screen.height/2-winMessage.height/2, winMessage.width, winMessage.height);
				GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox);
				GUI.DrawTexture(position, winMessage);
			} else if (playerHealth() <= 0) {
				Rect position = new Rect(Screen.width/2-loseMessage.width/2, Screen.height/2-loseMessage.height/2, loseMessage.width, loseMessage.height);
				GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackBox);
				GUI.Label(position, loseMessage);
			}
		}
	}
	
	void drawEnergyBar() {
		int energy = player.curPhoneCharge;
		int energyMax = player.maxPhoneCharge;
		float percent = ((float) energy) / energyMax;
		drawPercentBar(Screen.width - 200, 10, 150, 20, percent);
	}
	
	void drawPercentBar(int topLeftX, int topLeftY, int length, int height, float percent, string text = null) {
		GUI.DrawTexture(new Rect(topLeftX, topLeftY, length, height), blackBox);
		GUI.DrawTexture(new Rect(topLeftX + 5, topLeftY + 5, (length - (5*2))*percent, height - 5*2), blueBox);
		if (text != null) {
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.MiddleCenter;
			style.normal.textColor = Color.white;
			GUI.Label(new Rect(topLeftX + 5, topLeftY + 5, length - 5*2, height - 5*2), text, style);
		}
	}
	
	HackableComputer findCurrentHack() {
		foreach (HackableComputer comp in computerTerminals) {
			if (comp.beingHacked) {
				return comp;
			}
		}
		return null;
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
	
	public void pauseAndDrawTextMessage(string message) {
		currentMessage = message;
		isPaused = true;
		Time.timeScale = 0;
	}
	
	void drawTextMessage(string message) {
		GUIStyle style = new GUIStyle();
		style.normal.textColor = Color.black;
		//style.clipping = TextClipping.Clip;
		style.wordWrap = true;
		int upperLeftY = Screen.height - textMessageBox.height;
		int upperLeftX = (Screen.width - textMessageBox.width) / 2;
		GUI.DrawTexture(new Rect(upperLeftX, upperLeftY, textMessageBox.width, textMessageBox.height), textMessageBox);
		GUI.Label(new Rect(upperLeftX + 85, upperLeftY + 85, 370-5, (319-85)), message, style);
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
		return player.curHealth;
	}
}
