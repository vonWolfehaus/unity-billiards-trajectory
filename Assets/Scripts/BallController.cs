using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController:MonoBehaviour {

	LineRenderer lineRenderer;
	SimulatedPiece simulant;
	TrajectoryController trajController;
	List<Vector3> simplifiedPoints = new List<Vector3>();


	/**************************************
	 *	ACTIONS
	 */

	public void Reset() {
		// sync the hidden body with what the player is seeing in the active scene
		if (simulant) {
			simulant.hiddenBody.position = transform.position;
			simulant.hiddenBody.velocity = Vector3.zero;
			simulant.hiddenBody.angularVelocity = Vector3.zero;
		}
	}

	public void UpdateTrajectory(Vector3 force, int iterations, float physicsTimescale = 1) {
		// run the sim
		List<Vector3> points = simulant.SimulateImpulse(force, iterations, physicsTimescale);
		// reduce the number of points to use in the line before loading them in
		LineUtility.Simplify(points, 0.01f, simplifiedPoints);
		// set the line
		lineRenderer.positionCount = simplifiedPoints.Count;
		lineRenderer.SetPositions(simplifiedPoints.ToArray());
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
