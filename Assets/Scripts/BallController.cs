using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController:MonoBehaviour {

	LineRenderer lineRenderer;
	SimulatedPiece simulant;
	TrajectoryController trajController;


	/**************************************
	 *	ACTIONS
	 */

	public void Reset() {
		// sync the hidden body with what the player is seeing in the active scene
		if (simulant)
			simulant.hiddenBody.position = transform.position;
	}

	public void UpdateTrajectory(Vector3 force, int iterations, float physicsTimescale = 1) {
		Vector3[] points = simulant.SimulateImpulse(force, iterations, physicsTimescale);
		lineRenderer.positionCount = iterations;
		lineRenderer.SetPositions(points);
		lineRenderer.Simplify(0.01f);
	}

	public void OnSelect() {
		// immediately update the sim and display on select
		UpdateTrajectory(trajController.applyForce, trajController.simIterations, trajController.physicsTimescale);
		lineRenderer.enabled = true;
	}

	public void OnDeselect() {
		// hide the trajectory
		lineRenderer.enabled = false;
	}


	/**************************************
	 *	UNITY CALLBACKS
	 */

	void Start() {
		// setup references
		lineRenderer = GetComponent<LineRenderer>();
		simulant = GetComponent<SimulatedPiece>();
		trajController = (TrajectoryController)GameObject.Find("GameController").GetComponent<TrajectoryController>();

		// give the ball a random color
		GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
	}
}
