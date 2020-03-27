using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSelection:MonoBehaviour {

	[HideInInspector]
	public Rigidbody activeBody;

	List<BallController> allBalls;
	int clickedId = 0;
	bool isMouseDown = false;


	/**************************************
	 *	ACTIONS
	 */

	void ToggleBall(Rigidbody rigidbody, bool enable = true) {
		if (rigidbody == null)
			return;

		// reset the table so the new trajectory reflects the active scene
		foreach (BallController ball in allBalls) {
			ball.Reset();
		}

		BallController selected = (BallController)rigidbody.gameObject.GetComponent("BallController");
		if (enable) selected.OnSelect();
		else selected.OnDeselect();
	}


	/**************************************
	 *	UNITY CALLBACKS
	 */

	public void Start() {
		// with the scenes setup properly in TrajectoryController.Awake, we can start everything else
		allBalls = new List<BallController>();
		GameObject[] all = GameObject.FindGameObjectsWithTag("Ball");
		foreach (GameObject b in all) {
			allBalls.Add(b.GetComponent<BallController>());
		}
		// select the first ball on the table so there's always something to see
		activeBody = allBalls[0].GetComponent<Rigidbody>();
		ToggleBall(activeBody);
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
					// deselect current ball
					if (activeBody)
						ToggleBall(activeBody, false);

					// select the new one
					activeBody = hit.rigidbody;
					ToggleBall(activeBody);
				}
			}
		}
	}
}
