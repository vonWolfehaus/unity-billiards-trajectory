using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulatedPiece:MonoBehaviour {

	public Rigidbody hiddenBody;

	private PhysicsScene hiddenPhysicsScene;
	private Rigidbody activeBody;
	private bool isInitialized = false;

	// public void ResetPiece() {
	// 	hiddenBody.position = activeBody.position;
	// }

	public void SimulateImpulse(Vector3 force, int iterations, float physicsTimescale = 1) {
		hiddenBody.AddForce(force, ForceMode.Impulse);

		// now store all the positions into an array to use on the LineRenderer
		for (int i = 0; i < iterations; i++) {
			hiddenPhysicsScene.Simulate(Time.fixedDeltaTime * physicsTimescale);
			// Debug.Log(hiddenBody.position);
		}
	}

	void Update() {
		// TODO: make a public function that TrajectoryController can call whenever it's done making the scene
		// we can't use Start because the hidden scene hasn't been created yet
		if (!isInitialized) {
			TrajectoryController tc = (TrajectoryController)GameObject.Find("GameController").GetComponent<TrajectoryController>();
			// Debug.Log(TrajectoryController.hiddenSceneName);
			Scene hiddenScene = SceneManager.GetSceneByName(TrajectoryController.hiddenSceneName);
			// clone this object, grab a ref to its rb, then move it to the physics scene
			GameObject go = GameObject.Instantiate(gameObject, transform.position, Quaternion.identity);
			hiddenBody = go.GetComponent<Rigidbody>();
			// make sure we remove this script on the clone, or the clone will make more clones
			Destroy(go.GetComponent<SimulatedPiece>());

			if (CompareTag("Ball")) {
				// delete all the renderers and things we don't want messing things up
				Destroy(go.GetComponent<MeshRenderer>());
				Destroy(go.GetComponent<LineRenderer>());
				Destroy(go.GetComponent<BallController>());
			}
			else {
				// otherwise just hide it
				go.GetComponent<MeshRenderer>().enabled = false;
			}

			SceneManager.MoveGameObjectToScene(go, hiddenScene);

			activeBody = GetComponent<Rigidbody>();
			hiddenPhysicsScene = hiddenScene.GetPhysicsScene();

			isInitialized = true;
		}
	}
}
