using UnityEngine;
using System.Collections;

public class GroundControl : MonoBehaviour {
	
	/*
	 * 
	 */
	
	public tk2dSpriteCollection spriteCollection;
	
	public Texture[] sprites;
	public bool[] pathable;
	public int[,] pathableGrid = {
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
		{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
		{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
		{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
		{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
		{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
		{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
		{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
	};
	public int[,] groundTypes = {
		{2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 5},
		{6, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 6},
		{6, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 6},
		{6, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 6},
		{6, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 6},
		{6, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 6},
		{6, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 6},
		{6, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 6},
		{6, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 6},
		{9, 9, 9, 2, 4, 4, 4, 4, 4, 4, 4, 4, 5, 9, 9, 9},
		{9, 9, 9, 6, 0, 1, 0, 1, 1, 0, 1, 0, 6, 9, 9, 9},
		{9, 9, 9, 6, 1, 0, 1, 0, 0, 1, 0, 1, 6, 9, 9, 9},
		{9, 9, 9, 6, 0, 1, 0, 1, 1, 0, 1, 0, 6, 9, 9, 9},
		{9, 9, 9, 6, 1, 0, 1, 0, 0, 1, 0, 1, 6, 9, 9, 9},
		{9, 9, 9, 6, 0, 1, 0, 1, 1, 0, 1, 0, 6, 9, 9, 9},
		{9, 9, 9, 6, 1, 0, 1, 0, 0, 1, 0, 1, 6, 9, 9, 9}
	};
	
	
	int width;
	int height;
	int totalWidth;
	int totalHeight;
	int upperLeftX;
	int upperLeftY;
	
	CameraControl drawer;
	// Use this for initialization
	void Start () {
		drawer = Camera.main.GetComponent<CameraControl>();
		
		width = sprites[0].width;
		height = sprites[0].height;
		totalWidth = width * groundTypes.GetLength(1);
		totalHeight = height * groundTypes.GetLength(0);
		upperLeftX = -totalWidth/2;
		upperLeftY = totalHeight/2;
		Debug.Log("Upper left: " + upperLeftX+" "+upperLeftY);
		Debug.Log("Lower right: "+(upperLeftX+totalWidth)+" "+(upperLeftY-totalHeight));
		
		
/*		for (int r = 0; r < groundTypes.GetLength(0); r++) {
			for (int c = 0; c < groundTypes.GetLength(1); c++) {
				Vector3 worldCenter = worldCenterOf(r, c);
				//drawer.drawSpriteInWorld(sprites[groundTypes[r,c]], worldCenter.x, worldCenter.y);
				GameObject sprite = tk2dBaseSprite.CreateFromTexture<tk2dSprite>(sprites[groundTypes[r,c]], tk2dSpriteCollectionSize.PixelsPerMeter(1), new Rect(0,0,width,height), new Vector2(0,0));
				sprite.transform.position = worldCenterOf(r, c);
				sprite.transform.position += new Vector3(0, 0, 5);
				//tk2dSprite. spriteCollection
			}
		}*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/*void OnGUI() {
		GUI.depth = 10;
		for (int r = 0; r < groundTypes.GetLength(0); r++) {
			for (int c = 0; c < groundTypes.GetLength(1); c++) {
				Vector3 worldCenter = worldCenterOf(r, c);
				drawer.drawSpriteInWorld(sprites[groundTypes[r,c]], worldCenter.x, worldCenter.y);
			}
		}
	}*/
	
	Vector3 worldCenterOf(int row, int col) {
		return new Vector3(upperLeftX+(width*col)+(width/2), upperLeftY+(-height*row)-(height/2), 0);
	}
	
	Rect worldRectOf(int row, int col) {
		Vector3 center = worldCenterOf(row, col);
		return new Rect(center.x-width/2, center.y-height/2, width, height);
	}
	
	public bool pointPathable(Vector3 pos) {
		for (int r = 0; r < groundTypes.GetLength(0); r++) {
			for (int c = 0; c < groundTypes.GetLength(1); c++) {
				if (pathableGrid[r,c] == 0) {
					if (worldRectOf(r, c).Contains(pos)) {
						return false;
					}
				}
			}
		}
		return true;
	}
	
	//public bool rectIntersect(Rect r1, Rect r2) {
		
	//}
}
