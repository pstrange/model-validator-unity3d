using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomScript : MonoBehaviour {
	
	float minFov = 15f;
	float maxFov = 90f;
	float sensitivity = 10f;
	float startPosition;

	void Start(){
		startPosition = Camera.main.fieldOfView;
	}

	void Update () {
		float fov = Camera.main.fieldOfView;
		fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
		fov = Mathf.Clamp(fov, minFov, maxFov);
		Camera.main.fieldOfView = fov;
	}

	public void resetCamera(){
		Camera.main.fieldOfView = startPosition;
	}
}
