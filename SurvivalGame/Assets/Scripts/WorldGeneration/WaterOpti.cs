using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOpti : MonoBehaviour {

    PlayerManager manager;
    public bool removeWater;

    void Start()
    {
        manager = GameObject.FindObjectOfType<PlayerManager>();

        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.down);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.point.y < 125f)
            {
                removeWater = false;
            }
            else
            {
                removeWater = true;
            }
        }
        else
        {
            if(hit.transform == null)
            {
                removeWater = true;
            }
        }
    }

    private void Update()
    {
        if (manager.isUnderwater)
        {
            transform.localScale = new Vector3(transform.localScale.x, -1.0f, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, 1.0f, transform.localScale.z);
        }
    }
}
