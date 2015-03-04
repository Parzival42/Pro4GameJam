using UnityEngine;
using System.Collections;

public class SimpleAIAgent : MonoBehaviour {
	private GameObject[] players;
	private Transform dummyPlayer;		// Used to calculate the nearest player.
	private Transform player;
	private NavMeshAgent agent;
	private int playerIndex;
	private float distance = 0.0f;
	public int attackRange;

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

		if (distance < attackRange){
			GetComponent<Renderer>().material.color = Color.red;
			agent.SetDestination(player.position);
		}

	}
}
