using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSelection:MonoBehaviour {

	[HideInInspector]
	public Rigidbody activeBody;

	int clickedId = 0;
	bool isMouseDown = false;

	void Start() {
		GameObject ball = GameObject.FindWithTag("Ball");
		if (ball) {
			activeBody = ball.GetComponent<Rigidbody>();
			ToggleBall(activeBody);
		}
	}

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
					ToggleBall(activeBody, false);

					activeBody = hit.rigidbody;
					ToggleBall(activeBody);
				}
			}
		}
	}

	void ToggleBall(Rigidbody rigidbody, bool enable = true) {
		if (rigidbody == null)
			return;

		BallController ball = (BallController)rigidbody.transform.gameObject.GetComponent("BallController");
		if (enable) ball.OnSelect();
		else ball.OnDeselect();
	}
}
