using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailRoadInputController : MonoBehaviour {

	private SteamVR_Controller.Device controller {get { return SteamVR_Controller.Input ((int)trackedObj.index);	}	}

	private SteamVR_TrackedObject trackedObj;

	private GameObject currentHeldObject;

	private bool isCurrentlyHoldingAnObject = false;

	void Start() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void Update() {
		if (controller == null) {
			Debug.Log("Controller not initialized");
			return;
		}

		if (isCurrentlyHoldingAnObject) {
			currentHeldObject.transform.position = transform.position;
			currentHeldObject.transform.rotation = transform.rotation;

			Ray ray = new Ray (transform.position, Vector3.down);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 5f)) {
				if (hit.collider.CompareTag ("Terrain")) {
					currentHeldObject.transform.position = hit.collider.transform.position + Vector3.up * DataMaster.tileSize;

					if(transform.rotation.eulerAngles.y < 45 && transform.rotation.eulerAngles.y > 0 || transform.rotation.eulerAngles.y > 315 && transform.rotation.eulerAngles.y < 360){
						currentHeldObject.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
					}

					if(transform.rotation.eulerAngles.y < 135 && transform.rotation.eulerAngles.y > 45){
						currentHeldObject.transform.rotation = Quaternion.Euler (new Vector3 (0, 90, 0));
					}

					if(transform.rotation.eulerAngles.y < 225 && transform.rotation.eulerAngles.y > 135){
						currentHeldObject.transform.rotation = Quaternion.Euler (new Vector3 (0, 180, 0));
					}

					if(transform.rotation.eulerAngles.y < 315 && transform.rotation.eulerAngles.y > 225){
						currentHeldObject.transform.rotation = Quaternion.Euler (new Vector3 (0, 270, 0));
					}
				}
			}

			if (controller.GetHairTriggerDown ()) {
				RailRoadContainer tempRRC = hit.collider.GetComponent<RailRoadContainer> ();
				if (tempRRC.currentRailRoadPiece == null) {
					tempRRC.currentRailRoadPiece = currentHeldObject;
					currentHeldObject = null;
					isCurrentlyHoldingAnObject = false;
				}
			}
		}
	}

	void OnTriggerStay(Collider col){
		if (col.CompareTag ("Toolbox")) {
			if (currentHeldObject == null) {
				if (controller.GetHairTriggerDown ()) {	
					GameObject go = Instantiate (col.GetComponent<RailRoadToolbox> ().GetRailRoadPiece (), transform.position, transform.rotation);
					go.transform.localScale = Vector3.one * DataMaster.tileSize * 0.5f;
					currentHeldObject = go;
					Invoke ("GrabbedAnObject", 0.5f);
				}
			}
		}
	}

	void GrabbedAnObject(){
		isCurrentlyHoldingAnObject = true;
	}
}