using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewBurstTest : MonoBehaviour {
	GameObject screwBurstPrefab;

	// Use this for initialization
	void Start () {
		screwBurstPrefab = (GameObject)Resources.Load("ScrewBurst");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha5)) {
			GameObject.Instantiate(screwBurstPrefab, transform.position, transform.rotation);
		}
	}
}
