using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampPost : MonoBehaviour {

	GameObject sun;
	SunRotation sRot;

	public Transform[] lights;

	void Start ()
	{
		sun = GameObject.FindGameObjectWithTag ("Sun");
		sRot = sun.GetComponent<SunRotation> ();
	}

	void Update ()
	{
		if (sRot.currentTimeOfDay >= 0.25f && sRot.currentTimeOfDay <= 0.75f)
		{
			for (int i = 0; i < lights.Length; i++)
			{
				lights [i].gameObject.SetActive (false);
			}
		} 
		else 
		{
			for (int i = 0; i < lights.Length; i++)
			{
				lights [i].gameObject.SetActive (true);
			}
		}
	}
}
