using UnityEngine;
using System.Collections;

public class SmoothMovement : MonoBehaviour {

	//public float duration;

	NetworkView view;

	private Vector3 lastPosition;
	private Quaternion lastRotation;

	void Start ()
	{
		view = GetComponent<NetworkView> ();
	}

	void Update ()
	{
		if(Vector3.Distance(transform.position, lastPosition) >= 0.1)
		{
			lastPosition = transform.position;
			view.RPC("UpdateMovement", RPCMode.OthersBuffered, transform.position, transform.rotation);
		}

		if(Quaternion.Angle(transform.rotation, lastRotation) >= 1)
		{
			lastRotation = transform.rotation;   
			view.RPC("UpdateMovement", RPCMode.OthersBuffered, transform.position, transform.rotation);
		}
	}

	[RPC]
	void UpdateMovement (Vector3 newPosition, Quaternion newRotation)
	{
		transform.position = newPosition;
		transform.rotation = newRotation;
	}
}
