using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerManager : MonoBehaviour
{

    public NetworkView view;
    public int ID;
    public FirstPersonController controller;
    public PlayerManager manager;
    public Rigidbody rigidbody;
    public GameObject cam;
    public GameObject model;

    private Vector3 lastPosition;
    private Quaternion lastRotation;

    bool pauseMenu = false;

    public int health, maxHealth;

    void Start()
    {
        if (view.isMine)
        {
            transform.gameObject.tag = "MyPlayer";
            gameObject.layer = 8;
        }
    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu = !pauseMenu;
        }

        if (view.isMine)
        {
            controller.enabled = true;
            cam.SetActive(true);
            manager.enabled = true;
            //model.SetActive (false);
			/*model2.SetActive (false);
			model3.SetActive (false);
			thirdPersonGun.SetActive (false);*/
        }
        else
        {
            controller.enabled = false;
            cam.SetActive(false);
            manager.enabled = false;
            model.SetActive (true);
			/*model2.SetActive (true);
			model3.SetActive (true);
			thirdPersonGun.SetActive (true);*/
        }

        if (Vector3.Distance(transform.position, lastPosition) >= 0.1)
        {
            lastPosition = transform.position;
            view.RPC("UpdateMovement", RPCMode.OthersBuffered, transform.position, transform.rotation);
        }

        if (Quaternion.Angle(transform.rotation, lastRotation) >= 1)
        {
            lastRotation = transform.rotation;
            view.RPC("UpdateMovement", RPCMode.OthersBuffered, transform.position, transform.rotation);
        }
    }

    [RPC]
    void UpdateMovement(Vector3 newPosition, Quaternion newRotation)
    {
        transform.position = newPosition;
        transform.rotation = newRotation;
    }

    [RPC]
    void UpdateServerHealth(int newHealth)
    {
        health = newHealth;
    }

    public void UpdateHealth(int change)
    {
        health += change;
        view.RPC("UpdateServerHealth", RPCMode.All, health);
    }

    void OnDisconnectedFromServer()
    {
        Network.Destroy(gameObject);
    }

    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2 - 10, Screen.height / 2 - 10, 20, 20), "");
        GUI.Box (new Rect (0, 0, 120, 30), "Health: " + health + " / " + maxHealth);

        if (pauseMenu)
        {
            controller.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GUI.BeginGroup(new Rect(Screen.width / 2 - 125, Screen.height / 2 - 250, 250, 500), "");

            GUI.Box(new Rect(0, 0, 250, 500), "Paused");

            /*if (GUI.Button(new Rect(5, 30, 240, 30), "Disconnect"))
            {
                //Network.Destroy (view.viewID);
                Network.Disconnect();
                MasterServer.UnregisterHost();
            }*/

            GUI.EndGroup();
        }
        else
        {
            controller.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
