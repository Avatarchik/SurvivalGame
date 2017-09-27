using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPos : MonoBehaviour {

    public Transform player;

	void FixedUpdate ()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("MyPlayer").transform;
        }

        Vector3 pos = new Vector3(player.transform.position.x, 3.2f, player.transform.position.z);
        transform.position = pos;
	}
}
