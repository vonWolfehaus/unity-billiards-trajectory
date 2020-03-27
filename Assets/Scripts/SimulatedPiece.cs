using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulatedPiece:MonoBehaviour {

	[HideInInspector]
	public Rigidbody hiddenBody;

	private PhysicsScene hiddenPhysicsScene;
	private Rigidbody activeBody;


	/**************************************
	 *	ACTIONS
	 */

	public Vector3[] SimulateImpulse(Vector3 force, int iterations, float physicsTimescale = 1) {
		// with the board setup, we apply the force that will be applied to the real one if they launch it
		hiddenBody.AddForce(force, ForceMode.Impulse);
		// create a new array to store all the positions the hiddenBody generates as it travels in the sim
		// NOTE: in a production release you will need to use an object pool or pre-allocate the array to a fixed length.
		// never create new objects during runtime, or it will have a major performance impact
		Vector3[] points = new Vector3[iterations];
		for (int i = 0; i < iterations; i++) {
			// the magic line: run the hidden sim for iterations and save the results
			hiddenPhysicsScene.Simulate(Time.fixedDeltaTime * physicsTimescale);
			// now store the new position to use on the LineRenderer
			points[i] = hiddenBody.position;
		}

		return points;
	}


	/**************************************
	 *	UNITY CALLBACKS
	 */

	void Start() {
		activeBody = GetComponent<Rigidbody>();

		TrajectoryController tc = (TrajectoryController)GameObject.Find("GameController").GetComponent<TrajectoryController>();
		Scene hiddenScene = SceneManager.GetSceneByName(TrajectoryController.hiddenSceneName);
		hiddenPhysicsScene = hiddenScene.GetPhysicsScene();

		// clone this object, grab a ref to its rb, then move it to the physics scene
		GameObject go = GameObject.Instantiate(gameObject, transform.position, Quaternion.identity);
		// make sure we remove this script on the clone, or the clone will make more clones forever
		Destroy(go.GetComponent<SimulatedPiece>());

		if (CompareTag("Ball")) {
			// if this gameObject is a Ball, reference its rb
			hiddenBody = go.GetComponent<Rigidbody>();
			// and delete all its clone's renderers and things we don't want interfering
			Destroy(go.GetComponent<MeshRenderer>());
			Destroy(go.GetComponent<LineRenderer>());
			Destroy(go.GetComponent<BallController>());
		}
		else {
			// otherwise just hide it
			go.GetComponent<MeshRenderer>().enabled = false;
		}
		// now move the clone to the hidden physics scene where the sim will use it to create our trajectory prediction
		SceneManager.MoveGameObjectToScene(go, hiddenScene);
	}
}
