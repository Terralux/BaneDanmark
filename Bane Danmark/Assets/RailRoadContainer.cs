using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailRoadContainer : MonoBehaviour {

	[HideInInspector]
	public GameObject currentRailRoadPiece;

	public RailRoadContainer up;
	public RailRoadContainer down;
	public RailRoadContainer left;
	public RailRoadContainer right;

}