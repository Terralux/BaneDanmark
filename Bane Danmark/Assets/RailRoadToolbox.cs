using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailRoadToolbox : MonoBehaviour {

	public List<GameObject> railRoadPieces = new List<GameObject>();

	public GameObject GetRailRoadPiece(){
		return railRoadPieces [Random.Range (0, railRoadPieces.Count)];
	}
}