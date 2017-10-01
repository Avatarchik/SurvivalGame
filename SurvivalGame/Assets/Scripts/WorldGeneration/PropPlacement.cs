using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropPlacement : MonoBehaviour {

    Tree tree;
    RaycastHit hit;
    NetworkView view;

    Rigidbody rb;

    void Start()
    {
        tree = transform.parent.gameObject.GetComponent<Tree>();
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        rb.WakeUp();

        if (transform.parent.transform.position.y <= 15)
        {
            Network.Destroy(transform.parent.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Terrain")
        {
            tree.hitTerrain = true;
            Debug.Log("Tree hit Terrain");
            Destroy(gameObject);
        }
    }
}
