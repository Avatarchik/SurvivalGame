using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropPlacer : MonoBehaviour {

    public Vector2 coord;
    public List<GameObject> treeList;
    NetworkView view;

	void Start ()
    {
        view = GetComponent<NetworkView>();

        view.RPC("CreateTree", RPCMode.AllBuffered);
	}
	
	void Update () {
		
	}

    [RPC]
    void CreateTree()
    {
        Vector3 treePos = new Vector3(coord.x * 100 + Random.Range(-50, 50), 25, coord.y * 100 + Random.Range(-50, 50));
        Quaternion treeRot = new Quaternion(267f, 0, Random.Range(0, 360), 0);
        GameObject curTree = Network.Instantiate(treeList[0], treePos, treeRot, 0) as GameObject;
        curTree.transform.eulerAngles = new Vector3(treeRot.x, treeRot.y, treeRot.z);
        curTree.transform.parent = transform.parent;
    }

}
