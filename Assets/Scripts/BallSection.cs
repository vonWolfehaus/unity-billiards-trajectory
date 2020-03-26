using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSection:MonoBehaviour {

	// Rigidbody activeBody = null;
	int clickedId = 0;
	bool isMouseDown = false;

	// whenever the player clicks down AND up on the same ball (ie doesn't move mouse to orbit camera) then select that ball
	void Update() {
		RaycastHit hit;
		Ray ray;

		if (Input.GetMouseButtonDown(0)) {
			isMouseDown = true;

			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				clickedId = hit.rigidbody.GetInstanceID();
			}
		}
		else if (isMouseDown && Input.GetMouseButtonUp(0)) {
			isMouseDown = false;

			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				if (clickedId == hit.rigidbody.GetInstanceID()) {
					// activeBody = hit.rigidbody;
				}
			}
		}
	}
}
