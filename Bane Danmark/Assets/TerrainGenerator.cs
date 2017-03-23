using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

	public float xGridSize;
	public float yGridSize;

	public float tileSize;

	public int fillPercentage;

	public GameObject grass;
	public GameObject rocks;

	public int randomSeed;
	public int smoothing = 3;

	private Map myMap;

	private List<GameObject> objectsToDelete = new List<GameObject> ();

	void Start () {
		GenerateMap ();
	}

	void GenerateMap(){
		Random.InitState (randomSeed);

		foreach (GameObject go in objectsToDelete) {
			Destroy (go);
		}
		objectsToDelete.Clear ();

		myMap = new Map ((int)((xGridSize * 2) / (float)tileSize), (int)((yGridSize * 2) / (float)tileSize));
		myMap.UpdateMapValues (fillPercentage, smoothing);

		for (int i = 0; i < myMap.mapValues.GetLength (0); i++) {
			for (int j = 0; j < myMap.mapValues.GetLength (1); j++) {
				if (myMap.mapValues [i, j] != 0) {
					GameObject go = Instantiate (grass, new Vector3 (transform.position.x - xGridSize + (i * tileSize), transform.position.y, transform.position.z - yGridSize + (j * tileSize)), Quaternion.identity);

					go.transform.localScale = Vector3.one * tileSize;
					objectsToDelete.Add (go);
				} else {
					GameObject go = Instantiate (rocks, new Vector3 (transform.position.x - xGridSize + (i * tileSize), transform.position.y + tileSize, transform.position.z - yGridSize + (j * tileSize)), Quaternion.identity);

					go.transform.localScale = Vector3.one * tileSize;
					objectsToDelete.Add (go);
				}
			}
		}
	}

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			GenerateMap ();
		}
	}

	/*
	// Update is called once per frame
	void OnDrawGizmos () {
		Gizmos.DrawWireCube (transform.position, new Vector3 (xGridSize * 2, tileSize + (tileSize * 0.1f), yGridSize * 2));

		if (myMap == null) {
			return;
		}

		for (int i = 0; i < myMap.mapValues.GetLength (0); i++) {
			for (int j = 0; j < myMap.mapValues.GetLength (1); j++) {
				if (myMap.mapValues [i, j] != 0) {
					Gizmos.DrawCube (new Vector3 (transform.position.x - xGridSize + (i * tileSize), transform.position.y, transform.position.z - yGridSize + (j * tileSize)), Vector3.one*tileSize);
				} else {
					Gizmos.DrawWireCube (new Vector3 (transform.position.x - xGridSize + (i * tileSize), transform.position.y, transform.position.z - yGridSize + (j * tileSize)), Vector3.one*tileSize);
				}
			}
		}
	}
	*/
}

public class Map {

	public int[,] mapValues;

	public Map(int xTiles, int yTiles){
		mapValues = new int[xTiles, yTiles];
	}

	public void UpdateMapValues(int fillPercentage, int smoothIterations){
		for (int i = 0; i < mapValues.GetLength (0); i++) {
			for (int j = 0; j < mapValues.GetLength (1); j++) {
				int randomValue = Random.Range (0, 100);
				if (randomValue < fillPercentage) {
					mapValues [i, j]++;
				}
			}
		}

		for (int i = 0; i < smoothIterations; i++) {
			SmoothMap ();
		}
	}

	void SmoothMap() {
		for (int x = 0; x < mapValues.GetLength(0); x ++) {
			for (int y = 0; y < mapValues.GetLength(1); y ++) {
				int neighbourWallTiles = GetSurroundingWallCount(x,y);

				if (neighbourWallTiles > 4)
					mapValues[x,y] = 1;
				else if (neighbourWallTiles < 4)
					mapValues[x,y] = 0;

			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridY) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
				if (neighbourX >= 0 && neighbourX < mapValues.GetLength(0) && neighbourY >= 0 && neighbourY < mapValues.GetLength(1)) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += mapValues[neighbourX,neighbourY];
					}
				}
				else {
					wallCount ++;
				}
			}
		}

		return wallCount;
	}
}