using UnityEngine;
using System.Collections;

public class SimpleAIAgent : MonoBehaviour {
	private GameObject[] players;		// Array of active players in the game
	private Transform dummyPlayer;		// Used to calculate the nearest player.
	private Transform player;			// The target player
	private NavMeshAgent agent;			// The baked navigation mesh
	private int playerIndex;			// Index of the players array
	private float distance = 0.0f;		// Distance between player and enemy


	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		players = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		players = GameObject.FindGameObjectsWithTag("Player");
		player = players[0].transform;
		for (int i = 0; i < players.Length; i++){
			dummyPlayer = players[i].transform;
			if (distance > Vector3.Distance(dummyPlayer.position , transform.position)){
				player = dummyPlayer;
			}
			distance = Vector3.Distance(dummyPlayer.position , transform.position);
		}

		agent.SetDestination(player.position);

		/*
		if (distance < attackRange){
			GetComponent<Renderer>().material.color = Color.red;
			agent.SetDestination(player.position);
		}
		*/

	}
}
