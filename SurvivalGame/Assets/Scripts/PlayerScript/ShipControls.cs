using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ShipControls : MonoBehaviour {

    FirstPersonController controller;

    public float waterLevel;
    public bool playerBoarded;
    public bool playerAtWheel;

    public bool mainSail, topSail, topGallantSail, royalSail, skySail, moonRaker;

    public float speed = 10f;

	void Start ()
    {
        Vector3 startPos = new Vector3(transform.position.x, waterLevel, transform.position.z);
        transform.position = startPos;
    }

	void Update ()
    {
        if(controller == null)
        {
            controller = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        }

        if (playerAtWheel)
        {
            controller.enabled = false;
        }
    }

    private void FixedUpdate()
    {
       
        if (playerAtWheel)
        {
            if (Input.GetKey(KeyCode.A))
            {
                float shipRot = transform.localEulerAngles.y - 0.2f;

                Vector3 newRot = new Vector3(0f, shipRot, 0f);
                transform.localEulerAngles = newRot;
            }
            if (Input.GetKey(KeyCode.D))
            {
                float shipRot = transform.localEulerAngles.y + 0.2f;

                Vector3 newRot = new Vector3(0f, shipRot, 0f);
                transform.localEulerAngles = newRot;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                mainSail = true;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                mainSail = false;
            }
        }

        if (mainSail)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
