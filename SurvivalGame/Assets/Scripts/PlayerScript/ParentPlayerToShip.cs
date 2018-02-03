using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPlayerToShip : MonoBehaviour {

    ShipManager currentShip;

    void Start ()
    {
        currentShip = transform.parent.gameObject.GetComponent<ShipManager>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.parent = this.transform.parent;
            currentShip.playerBoarded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.parent = null;
            currentShip.playerBoarded = false;
        }
    }
}
