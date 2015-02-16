﻿using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour 
{
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
            obj.transform.position = new Vector3(-165, 1, 0);
        }
    }
}