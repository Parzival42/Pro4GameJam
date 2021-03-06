﻿using UnityEngine;
using System.Collections;
using System.IO;

public class PlayerManager : MonoBehaviour 
{
    Transform spawnPosition;

    //Actual player count (starts with one)
    private int playerCount = 1;
    public int PlayerCount
    {
        get { return playerCount; }
    }

    /// <summary>
    /// Returns the player Prefix for the controlls
    /// </summary>
    public string PlayerPrefix
    {
        get { return "P" + playerCount + "_"; }
    }


	// Use this for initialization
	void Start () 
    {

		if (GameObject.FindGameObjectsWithTag ("PlayerSpawn").Length > 0) {
			spawnPosition = GameObject.FindGameObjectsWithTag("PlayerSpawn")[0].transform;
		}

	}
	
	// Update is called once per frame
	void Update () 
    {
        HandlePlayerJoin();
	}

    private void HandlePlayerJoin()
    {
        GameObject obj;

        if (Input.GetAxis("P2_Join") > 0 && playerCount == 1)
        {
            Debug.Log("Player 2 pressed Join!");

            playerCount++;
            obj = Instantiate(Resources.Load<GameObject>("Player")) as GameObject;
            obj.transform.position = spawnPosition.position;
        }

    }

	public void HandlePhonePlayerJoin(int PLAYER) {

		GameObject obj;

		if (playerCount == 1 && Input.GetJoystickNames ().Length == 0 && PLAYER == 0) {

			Debug.Log("Player 1 joined with phone!");
			SimplePlayer p = GameObject.FindObjectOfType<SimplePlayer>();
			p.PhonePlayer = PLAYER;
			p.IsPhone = true;

		} else if (playerCount == 1) {
			
			Debug.Log("Player 2 joined with phone!");
			obj = Instantiate(Resources.Load<GameObject>("Player")) as GameObject;
			obj.GetComponent<SimplePlayer>().PhonePlayer = PLAYER;
			obj.GetComponent<SimplePlayer>().IsPhone = true;
			obj.transform.position = spawnPosition.position;
			
		} else if (playerCount == 2) {
			
			Debug.Log("Player 3 joined with phone!");
			obj = Instantiate(Resources.Load<GameObject>("Player")) as GameObject;
			obj.GetComponent<SimplePlayer>().PhonePlayer = PLAYER;
			obj.GetComponent<SimplePlayer>().IsPhone = true;
			obj.transform.position = spawnPosition.position;
			
		} else if (playerCount == 3) {
			
			Debug.Log("Player 4 joined with phone!");
			obj = Instantiate(Resources.Load<GameObject>("Player")) as GameObject;
			obj.GetComponent<SimplePlayer>().PhonePlayer = PLAYER;
			obj.GetComponent<SimplePlayer>().IsPhone = true;
			obj.transform.position = spawnPosition.position;
			
		}

		playerCount++;

	}

}
