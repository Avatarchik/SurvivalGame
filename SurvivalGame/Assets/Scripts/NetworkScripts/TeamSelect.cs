using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelect : MonoBehaviour {

    NetworkView view;

    public NetworkManager nManager;
    public bool showTeams = false;

	void Start ()
    {
        view = GetComponent<NetworkView>();

        nManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<NetworkManager>();
        showTeams = true;

        if (!view.isMine)
        {
            gameObject.SetActive(false);
        }
	}
	void Update ()
    {
		
	}

    private void OnGUI()
    {
        if (showTeams)
        {
            GUI.BeginGroup(new Rect(0, 0, 150, 150), "");

            GUI.Box(new Rect(0, 0, 150, 150), "Select Team");

            if (GUI.Button(new Rect(5, 25, 70, 30), "team 1"))
            {
                if(nManager.team1Amount != nManager.team1AmountMax)
                {
                    nManager.UpdateTeamCount(1, 0);
                    nManager.SpawnPlayer();
                    Network.Destroy(gameObject);
                }
            }
            GUI.Label(new Rect(5, 55, 70, 30), "t1:" + nManager.team1Amount + "/" + nManager.team1AmountMax);

            if (GUI.Button(new Rect(75, 25, 70, 30), "team 2"))
            {
                if (nManager.team2Amount != nManager.team2AmountMax)
                {
                    nManager.UpdateTeamCount(0, 1);
                    nManager.SpawnPlayer();
                    Network.Destroy(gameObject);
                }
            }
            GUI.Label(new Rect(75, 55, 70, 30), "t2:" + nManager.team2Amount + "/" + nManager.team2AmountMax);

            GUI.EndGroup();
        }
    }
}
