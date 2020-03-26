using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController:MonoBehaviour {

	[HideInInspector]
	public Rigidbody hiddenBody;

	LineRenderer lineRenderer;
	SimulatedPiece simulant;
	int lengthOfLineRenderer = 20;


	/**************************************
	 *	ACTIONS
	 */

	public void UpdateTrajectory(Vector3 force, int iterations, float physicsTimescale = 1) {
		simulant.SimulateImpulse(force, iterations, physicsTimescale);
	}

	public void OnSelect() {
		lineRenderer.enabled = true;
		// simulant.SimulateImpulse();
	}

	public void OnDeselect() {
		lineRenderer.enabled = false;
	}


	/**************************************
	 *	UNITY CALLBACKS
	 */

	void Start() {
		lineRenderer = GetComponent<LineRenderer>();
		simulant = GetComponent<SimulatedPiece>();

		// give the ball a random color
		GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
	}

	void DoThing() {
        Vector3[] points = new Vector3[lengthOfLineRenderer];
        float t = Time.time;
		int i;
        for (i = 0; i < lengthOfLineRenderer; i++) {
            points[i] = new Vector3(i * 0.5f, Mathf.Sin(i + t), 0.0f);
        }
        lineRenderer.SetPositions(points);
    }
}
