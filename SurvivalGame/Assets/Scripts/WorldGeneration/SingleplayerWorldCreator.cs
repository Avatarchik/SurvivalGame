﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleplayerWorldCreator : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawn;

    public GameObject sun;

    private string worldName = "World";
    public int seed;

    public bool inGame = false;
    public bool worldLoader = false;
    public bool createWorld = false;

    public HeightMapSettings heightMapSettings;

    const float normalScale = 1500;
    const int normalOctaves = 15;
    const float normalPersistance = 0.47f;
    const float normalLacunarity = 2.1f;
    const float normalHeightMulti = 500;

    float scale = 1500;
    int octaves = 15;
    float persistance = 0.47f;
    float lacunarity = 2.1f;
    float heightMulti = 500;


    private void Start()
    {
        seed = 0;

        heightMapSettings.noiseSettings.scale = normalScale;
        heightMapSettings.noiseSettings.octaves = normalOctaves;
        heightMapSettings.noiseSettings.persistance = normalPersistance;
        heightMapSettings.noiseSettings.lacunarity = normalLacunarity;
        heightMapSettings.heightMultiplier = normalHeightMulti;
    }

    private void CreateWorld()
    {
        if (seed == 0)
        {
            seed = Random.Range(1, int.MaxValue);
            Debug.Log("Seed has been randomly assigned to: " + seed);
        }

        heightMapSettings.noiseSettings.seed = seed;
        heightMapSettings.noiseSettings.scale = scale;
        heightMapSettings.noiseSettings.octaves = octaves;
        heightMapSettings.noiseSettings.persistance = persistance;
        heightMapSettings.noiseSettings.lacunarity = lacunarity;
        heightMapSettings.heightMultiplier = heightMulti;

        Debug.Log("Seed has set to: " + heightMapSettings.noiseSettings.seed);

        SpawnPlayer();

        inGame = true;
        createWorld = false;
    }

    public void SpawnPlayer()
    {
        Instantiate(playerPrefab, spawn.position, Quaternion.identity);
        //sun.GetComponent<AuraAPI.AuraLight>().enabled = true;
    }

    void OnGUI()
    {

        if (inGame == false)
        {

            if (GUI.Button(new Rect(5, 5, 250, 40), "Create World(Old News)"))
            {
                createWorld = true;
            }

            if (Application.loadedLevel == 0)
            {
                if (GUI.Button(new Rect(5, 95, 250, 40), "Performance Test"))
                {
                    Application.LoadLevel(2);
                }
            }
            else
            {
                if (GUI.Button(new Rect(5, 95, 250, 40), "Game Scene"))
                {
                    Application.LoadLevel(0);
                }
            }

            if (GUI.Button(new Rect(5, 140, 250, 40), "Network Prototype"))
            {
                Application.LoadLevel(1);
            }
            if (GUI.Button(new Rect(5, 185, 250, 40), "Prototype Scene"))
            {
                Application.LoadLevel(3);
            }
            if (GUI.Button(new Rect(5, 230, 250, 40), "Nothing to see here"))
            {
                //Application.LoadLevel(4);
            }
            if (GUI.Button(new Rect(5, 275, 250, 40), "Quit Game"))
            {
                Application.Quit();
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

                if (GUI.Button(new Rect(5, 95, 120, 30), "Test Seed"))
                {
                    seed = 1771046141;
                }

                /*GUI.Label(new Rect(105, 95, 100, 30), "Risky Settings");


                GUI.Label(new Rect(5, 130, 100, 50), "Noise Scale: \n" + scale);
                scale = GUI.HorizontalSlider(new Rect(105, 130, 100, 30), scale, 450.0f, 5000.0f);

                GUI.Label(new Rect(5, 165, 100, 30), "Octaves:");
                int.TryParse(GUI.TextField(new Rect(105, 165, 100, 30), octaves.ToString()), out octaves);

                GUI.Label(new Rect(5, 200, 100, 50), "Persistance: \n" + persistance);
                persistance = GUI.HorizontalSlider(new Rect(105, 200, 100, 30), persistance, 0.1f, 1.0f);

                GUI.Label(new Rect(5, 255, 100, 50), "Lacunarity: \n" + lacunarity);
                lacunarity = GUI.HorizontalSlider(new Rect(105, 255, 100, 30), lacunarity, 1.0f, 4.0f);

                GUI.Label(new Rect(5, 310, 100, 50), "Height Scale: \n" + heightMulti);
                heightMulti = GUI.HorizontalSlider(new Rect(105,  310, 100, 30), heightMulti, 50.0f, 1000.0f);*/

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
