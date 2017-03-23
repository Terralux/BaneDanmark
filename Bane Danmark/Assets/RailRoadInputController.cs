using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailRoadInputController : MonoBehaviour {

	private SteamVR_Controller.Device controller {get { return SteamVR_Controller.Input ((int)trackedObj.index);	}	}

	private SteamVR_TrackedObject trackedObj;

	private GameObject currentHeldObject;

	void Start() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void Update() {
		if (controller == null) {
			Debug.Log("Controller not initialized");
			return;
		}

		if (currentHeldObject != null) {
			Collider[] targets = Physics.OverlapBox (transform.position, Vector3.one * 0.2f);
			if (targets.Length > 0) {
				float minDistance = 1000f;
				int count = 0;
				int targetIndex = 0;
				foreach (Collider c in targets) {
					float testDistance = Vector3.Distance (c.transform.position, transform.position);
					if (testDistance < minDistance) {
						minDistance = testDistance;
						targetIndex = count;
					}
					count++;
				}
				currentHeldObject.transform.position = targets [targetIndex].transform.position;
			}

			if (controller.GetHairTriggerDown ()) {
				//place object
			}
		}
	}

	void OnTriggerStay(Collider col){
		if (col.CompareTag ("Toolbox")) {
			if (controller.GetHairTriggerDown ()) {	
				GameObject go = Instantiate (col.GetComponent<RailRoadToolbox> ().GetRailRoadPiece (), transform.position, transform.rotation);
				go.transform.SetParent (transform);
				currentHeldObject = go;
			}
		}
	}
}