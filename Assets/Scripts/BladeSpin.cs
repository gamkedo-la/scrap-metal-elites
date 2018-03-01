using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeSpin : MonoBehaviour
{
    public float spinSpeed = 5.0f;
    
    
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButton("Fire1"))
        {
            Debug.Log("mouse down");
            transform.Rotate(Vector3.forward * Time.deltaTime * -spinSpeed);
        }
	}
}
