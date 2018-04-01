using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {
	public float aliveTime = 4.0f;

	void Start () {
		Destroy(gameObject, aliveTime);
	}
}
