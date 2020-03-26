using UnityEngine;
using System.Collections;

public class OrbitController:MonoBehaviour {

	public Transform target;

	public float distance = 20f;
	public float xSpeed = 6.0f;
	public float ySpeed = 70.0f;
	public float distanceMin = 1f;
	public float distanceMax = 30f;

	float yMinLimit = 10f;
	float yMaxLimit = 80f;
	float x = 0.0f;
	float y = 0.0f;

	void Start() {
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
	}

	void LateUpdate() {
		if (target) {
			if (Input.GetMouseButton(0)) {
				x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
				y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
				y = ClampAngle(y, yMinLimit, yMaxLimit);
			}
			Quaternion rotation = Quaternion.Euler(y, x, 0);

			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;

			transform.rotation = rotation;
			transform.position = position;
		}
	}

	public static float ClampAngle(float angle, float min, float max) {
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}