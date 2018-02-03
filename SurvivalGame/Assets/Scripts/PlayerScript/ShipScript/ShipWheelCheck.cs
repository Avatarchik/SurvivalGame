using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWheelCheck : MonoBehaviour {

    ShipManager sManager;

    void Start ()
    {
        sManager = transform.parent.gameObject.GetComponent<ShipManager>();	
	}

    void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            sManager.playerAtWheel = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            sManager.playerAtWheel = false;
        }
    }
}
