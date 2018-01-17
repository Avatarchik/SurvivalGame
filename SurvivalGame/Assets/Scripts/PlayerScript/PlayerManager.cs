using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerManager : MonoBehaviour
{

    public SingleplayerWorldCreator worldCreator;

    public FirstPersonController controller;
    public PlayerManager manager;
    public Rigidbody rigidbody;
    public GameObject cam;

    public Inventory inv;

    bool pauseMenu = false;

    public int health, maxHealth;

    void Start()
    {
        worldCreator = GameObject.FindGameObjectWithTag("GameController").GetComponent<SingleplayerWorldCreator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click!");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10f))
            {
                Debug.DrawRay(transform.position, Vector3.forward, Color.red, 3.0f);
                Debug.Log("Ray hitting something");

                if (hit.transform.gameObject.tag == "Terrain")
                {
                    Debug.Log("Player Hit Terrain");
                }
                else if (hit.transform.gameObject.tag == "SmallRock")
                {
                    Debug.Log("Hit Small Rock");
                    int amount = Random.Range(1, 10);
                    inv.AddItem(1, amount);
                    Destroy(hit.transform.gameObject);
                }
                else
                {
                    Debug.Log("hit something that wasnt important, i hit: " + hit.transform.gameObject.tag);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu = !pauseMenu;
        }
    }

    public void UpdateHealth(int change)
    {
        health += change;
    }

    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2 - 10, Screen.height / 2 - 10, 20, 20), "");
        GUI.Box (new Rect (0, 0, 120, 30), "Health: " + health + " / " + maxHealth);

        if (pauseMenu)
        {
            /*controller.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;*/

            GUI.BeginGroup(new Rect(Screen.width / 2 - 125, Screen.height / 2 - 250, 250, 500), "");

            GUI.Box(new Rect(0, 0, 250, 500), "Paused");

            if (GUI.Button(new Rect(5, 30, 240, 30), "Quit"))
            {
                worldCreator.inGame = false;
                Destroy(gameObject);
            }

            GUI.EndGroup();
        }
        else
        {
            /*controller.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;*/
        }
    }
}
