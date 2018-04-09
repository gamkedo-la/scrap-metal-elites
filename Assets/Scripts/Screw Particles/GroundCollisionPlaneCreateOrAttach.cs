using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollisionPlaneCreateOrAttach : MonoBehaviour {
	private static Transform groundCollisionPlane;
	private ParticleSystem psScript;
	void Start () {
		psScript = GetComponent<ParticleSystem>();
		//Debug.Log(groundCollisionPlane);
		if(groundCollisionPlane == null) {
			GameObject groundGO = new GameObject();
			groundGO.name = "Ground for screws";
			groundGO.transform.position = Vector3.up * -4.602552f; // arena Y
			groundCollisionPlane = groundGO.transform;
		}
		psScript.collision.SetPlane(0, groundCollisionPlane);

		transform.SetParent(groundCollisionPlane); // keeps Hierarchy tidy
	}
}
