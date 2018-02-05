using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleplayerWorldCreator : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawn;

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
            if (GUI.Button(new Rect(5, 95, 250, 40), "Quit Game"))
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
