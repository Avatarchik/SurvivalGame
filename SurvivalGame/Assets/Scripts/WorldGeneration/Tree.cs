using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public bool debug = false;

    RaycastHit hit;
    NetworkView view;

    public bool hitTerrain = false;

    public Vector3 rayOrigin;

    public LayerMask mask;

    void Start ()
    {
        /*MeshCollider col = GetComponent<MeshCollider>();
        Debug.Log("Tree Initialised");

        view = GetComponent<NetworkView>();
        Vector3 rayStart = new Vector3(transform.position.x + rayOrigin.x, transform.position.y + rayOrigin.y, transform.position.z + rayOrigin.z);

	    if(Physics.Raycast(transform.position, Vector3.down, out hit, 1000f))
        {
            Debug.Log("Ray hitting something");
            if(hit.collider.tag == "Terrain")
            {
                Debug.Log("Tree Hit Terrain");
                col.enabled = true;
            }
            else if(hit.collider.tag != "Terrain")
            {
                Debug.Log("hit something that wasnt terrain, i hit: " + hit.transform.gameObject.tag);
                Network.Destroy(gameObject);
                Debug.Log("Destroyed Tree!");
            }

            Debug.Log("Tree at: " + transform.position + " hit at: " + hit.point);

        }
        else if(hit.collider == null)
        {
            Debug.Log("Hit nothing");
        }*/


	}
	void Update ()
    {
        if (!hitTerrain)
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            transform.position = newPos;
        }
    }
}
