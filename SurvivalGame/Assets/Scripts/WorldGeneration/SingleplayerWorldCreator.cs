using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleplayerWorldCreator : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawn;

    private string worldName = "World";
    public int seed;

    bool inGame = false;
    public bool worldLoader = false;
    public bool createWorld = false;

    public HeightMapSettings heightMapSettings;

    private void Start()
    {
        seed = heightMapSettings.noiseSettings.seed;
    }

    private void CreateWorld()
    {
        heightMapSettings.noiseSettings.seed = seed;

        if (seed == 0)
        {
            seed = Random.Range(1, int.MaxValue);
            heightMapSettings.noiseSettings.seed = seed;
        }
        SpawnPlayer();

        inGame = true;
    }

    public void SpawnPlayer()
    {
        Instantiate(playerPrefab, spawn.position, Quaternion.identity);
    }

    void OnGUI()
    {

        if (inGame == false)
        {

            if (GUI.Button(new Rect(5, 5, 250, 40), "Create World"))
                createWorld = true;

            if (GUI.Button(new Rect(5, 50, 250, 40), "World Browser"))
            {
                worldLoader = true;
            }

            if (createWorld)
            {
                GUI.BeginGroup(new Rect(Screen.width / 2 - 175, Screen.height / 2 - 250, 350, 500), "");
                GUI.Box(new Rect(0, 0, 350, 500), "Create World");

                if (GUI.Button(new Rect(325, 5, 20, 20), "X"))
                {
                    createWorld = false;
                }

                GUI.Label(new Rect(5, 25, 100, 30), "World Name:");
                worldName = GUI.TextField(new Rect(105, 25, 120, 30), worldName, 15);

                GUI.Label(new Rect(5, 60, 100, 30), "World Seed:");
                int.TryParse(GUI.TextField(new Rect(105, 60, 100, 30), seed.ToString()), out seed);

                if (GUI.Button(new Rect(225, 465, 120, 30), "Create World"))
                    CreateWorld();

                GUI.EndGroup();
            }

            if (worldLoader)
            {
                GUI.BeginGroup(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 500), "");

                GUI.Box(new Rect(0, 0, 500, 500), "Server Browser");

                if (GUI.Button(new Rect(475, 5, 20, 20), "X"))
                {
                    worldLoader = false;
                }

                GUI.EndGroup();
            }
        }
    }
}
