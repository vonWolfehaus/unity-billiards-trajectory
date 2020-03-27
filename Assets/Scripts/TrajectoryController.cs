using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectoryController:MonoBehaviour {

	[HideInInspector]
	public static string hiddenSceneName = "TrajectoryScene";
	[HideInInspector]
	public Vector3 applyForce = new Vector3();

	[Range(0.0f, 30.0f)]
	public float horizontalForce = 10f;
	[Range(0.0f, 30.0f)]
	public float verticalForce = 0f;
	public float forceAngle = 1.58f;
	public float physicsTimescale = 1f;
	[Range(50, 500)]
	public int simIterations = 250;

	float prevHorForce = 0f;
	float prevVertForce = 0f;
	float prevAngle = 0f;

	BallSelection selection;
	List<BallController> allBalls;
	Scene mainScene;
	Scene trajectoryScene;
	PhysicsScene trajectoryPhysicsScene;
	PhysicsScene mainScenePhysics;


	/**************************************
	 *	ACTIONS
	 */

	void UpdateTrajectory() {
		// reset the (hidden) scene to match the active scene (the table never moves so only worry about the balls)
		foreach (BallController ball in allBalls) {
			ball.Reset();
		}
		// update the force we'll apply to the active ball
		applyForce.Set(horizontalForce * Mathf.Cos(forceAngle), verticalForce, horizontalForce * Mathf.Sin(forceAngle));
		prevAngle = forceAngle;
		prevHorForce = horizontalForce;
		prevVertForce = verticalForce;
		// make sure the scenes are valid and a ball is selected
		if (!mainScenePhysics.IsValid() || !trajectoryPhysicsScene.IsValid() || selection.activeBody == null)
			return;

		BallController bc = selection.activeBody.gameObject.GetComponent<BallController>();
		bc.UpdateTrajectory(applyForce, simIterations, physicsTimescale);
	}


	/**************************************
	 *	UNITY CALLBACKS
	 */

	void Awake() {
		Physics.autoSimulation = false; // this means we have to run the physics scene(s) manually now in our own FixedUpdate
		mainScene = SceneManager.GetActiveScene();
		mainScenePhysics = mainScene.GetPhysicsScene();

		// you must create a scene with the LocalPhysicsMode.Physics3D parameter or the Simulate (float time) method for the trajectoryScene scene will simulate physics for all physical scenes
		CreateSceneParameters sceneParam = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
		trajectoryScene = SceneManager.CreateScene(hiddenSceneName, sceneParam);
		trajectoryPhysicsScene = trajectoryScene.GetPhysicsScene();

		selection = GetComponent<BallSelection>();
		allBalls = new List<BallController>();
		GameObject[] all = GameObject.FindGameObjectsWithTag("Ball");
		foreach (GameObject b in all) {
			allBalls.Add(b.GetComponent<BallController>());
		}
	}

	void FixedUpdate() {
		if (!mainScenePhysics.IsValid())
			return;

		mainScenePhysics.Simulate(Time.fixedDeltaTime * physicsTimescale);
	}

	void Update() {
		if (forceAngle != prevAngle || horizontalForce != prevHorForce || verticalForce != prevVertForce) {
			// the user changed something in the inspector, so update the sim
			UpdateTrajectory();
		}

		if (Input.GetMouseButtonDown(1)) {
			selection.activeBody.AddForce(applyForce, ForceMode.Impulse);
		}
	}
}
