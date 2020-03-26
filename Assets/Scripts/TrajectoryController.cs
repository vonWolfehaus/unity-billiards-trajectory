using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectoryController:MonoBehaviour {

	[HideInInspector]
	public static string hiddenSceneName = "TrajectoryScene";

	[Range(0.0f, 30.0f)]
	public float horizontalForce = 10f;
	[Range(0.0f, 30.0f)]
	public float verticalForce = 0f;
	public float forceAngle = 1.58f;
	public float physicsTimescale = 1f;
	[Range(10, 300)]
	public int simIterations = 100;

	float prevHorForce = 10f;
	float prevVertForce = 0f;
	float prevAngle = 1.58f;

	private Vector3 applyForce = new Vector3();
	private BallSelection selection;
	private Scene mainScene;
	private Scene trajectoryScene;
	private PhysicsScene trajectoryPhysicsScene;
	private PhysicsScene mainScenePhysics;


	/**************************************
	 *	ACTIONS
	 */

	void UpdateImpulse() {
		// update the force we'll apply to the active ball
		applyForce.Set(horizontalForce * Mathf.Cos(forceAngle), verticalForce, horizontalForce * Mathf.Sin(forceAngle));
		prevAngle = forceAngle;
		prevHorForce = horizontalForce;
		prevVertForce = verticalForce;
	}

	void UpdateTrajectory() {
		if (!mainScenePhysics.IsValid() || !trajectoryPhysicsScene.IsValid())
			return;

		BallController bc = selection.activeBody.gameObject.GetComponent<BallController>();
		bc.UpdateTrajectory(applyForce, simIterations, physicsTimescale);
	}


	/**************************************
	 *	UNITY CALLBACKS
	 */

	void Start() {
		Physics.autoSimulation = false;
		mainScene = SceneManager.GetActiveScene();
		mainScenePhysics = mainScene.GetPhysicsScene();

		// You must create a scene with the LocalPhysicsMode.Physics3D parameter,
		// If this is not done, then the Simulate (float time) method for the trajectoryScene scene will simulate physics for all physical scenes.
		CreateSceneParameters sceneParam = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
		trajectoryScene = SceneManager.CreateScene(hiddenSceneName, sceneParam);
		trajectoryPhysicsScene = trajectoryScene.GetPhysicsScene();

		selection = GetComponent<BallSelection>();

		UpdateImpulse();
	}

	void FixedUpdate() {
		if (!mainScenePhysics.IsValid())
			return;

		mainScenePhysics.Simulate(Time.fixedDeltaTime * physicsTimescale);
	}

	void Update() {
		if (forceAngle != prevAngle || horizontalForce != prevHorForce || verticalForce != prevVertForce) {
			// the user changed something in the inspector, so update the sim
			UpdateImpulse();
			UpdateTrajectory();
		}

		if (Input.GetMouseButtonDown(1) && selection.activeBody) {
			selection.activeBody.AddForce(applyForce, ForceMode.Impulse);
		}
	}
}
