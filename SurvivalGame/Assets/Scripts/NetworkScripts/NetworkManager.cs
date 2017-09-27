﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	public GameObject clientPrefab;
	public Transform spawn;

	string username = "";
	public List<PlayerData> playerData;

	private const string typeName = "RhysSamSurvival";
	private string gameName = "RoomName";
    private int serverCapacity = 12;

	private HostData[] hostList;

	public bool showBrowser = false;
	public bool createServer = false;

    private void StartServer()
	{
		Network.InitializeServer (serverCapacity, 25459, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (typeName, gameName);
	}

	void OnServerInitialized()
	{
		Debug.Log ("Server opened");
        SpawnPlayer();
	}

	private void RefreshHostList()
	{
		MasterServer.RequestHostList (typeName);
	}

    void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList ();
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect (hostData);
	}

	void OnConnectedToServer()
	{
		Debug.Log ("Connected to server");
        SpawnPlayer();
	}
    public void SpawnPlayer()
    {
        Network.Instantiate(clientPrefab, spawn.position, Quaternion.identity, 0);
    }

	void OnGUI()
	{

		if (!Network.isClient && !Network.isServer) 
		{
			
			if (GUI.Button (new Rect (5, 5, 250, 40), "Create Lobby"))
				createServer = true;

			if (GUI.Button (new Rect (5, 50, 250, 40), "Server Browser")) 
			{
				showBrowser = true;
				RefreshHostList ();
			}

			username = GUI.TextField (new Rect (5, 95, 150, 25), username);
			if (username == "") 
			{
				GUI.Label (new Rect (15, 95, 150, 30), "Username");
			}

			if (createServer)
			{
				GUI.BeginGroup (new Rect (Screen.width / 2 - 175, Screen.height / 2 - 250, 350, 500), "");
				GUI.Box (new Rect (0, 0, 350, 500), "Create Lobby");

				if(GUI.Button (new Rect (325, 5, 20, 20), "X"))
				{
					createServer = false;
				}

				GUI.Label(new Rect(5, 25, 100, 30), "Lobby Name:");
				gameName = GUI.TextField (new Rect (105, 25, 120, 30), gameName, 15);

				if (GUI.Button (new Rect (225, 465, 120, 30), "Create Server"))
					StartServer ();
				
				GUI.EndGroup();
			}

			if (showBrowser)
			{
				GUI.BeginGroup (new Rect (Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 500), "");

				GUI.Box (new Rect (0, 0, 500, 500), "Server Browser");

				if (GUI.Button (new Rect (455, 5, 20, 20), "RE"))
				{
					RefreshHostList ();
				}
				if(GUI.Button (new Rect (475, 5, 20, 20), "X"))
				{
					showBrowser = false;
				}

				if (hostList != null) 
				{
					for (int i = 0; i < hostList.Length; i++) 
					{
						if (GUI.Button (new Rect (15, 30 + (i * 31), 120, 30), hostList [i].gameName))
						{
							JoinServer (hostList [i]);
						}
					}
				}

				GUI.EndGroup ();
			}
		}
	}
}
