using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAssetsScript : MonoBehaviour {

	public GameObject startPosition;
	Transform storeTransform;

	void Start () {
		storeTransform = gameObject.GetComponent<Transform> ();
		resetPosition ();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0))
			if(startPosition != null)
				storeTransform.Rotate(startPosition.transform.rotation.y, -Input.GetAxis("Mouse X")*2, 0);
	}

	public void resetPosition ()
	{
		if (storeTransform != null) {
			storeTransform.position = startPosition.GetComponent<Transform> ().position;
			storeTransform.rotation = startPosition.GetComponent<Transform> ().rotation;
		}
	}

}
