using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterUpforce : MonoBehaviour {

    Rigidbody rb;
    public float waterLevel = 94.6f;
    public float thrust = 10f;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	void Update ()
    {
		if(transform.position.y < waterLevel)
        {
            rb.AddForce(Vector3.up * thrust);
        }
	}
}
