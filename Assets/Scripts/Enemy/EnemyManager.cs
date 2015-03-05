﻿using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	//public PlayerHealth playerHealth;       // Reference to the player's heatlh.
	public GameObject enemy;                // The enemy prefab to be spawned.
	public float spawnTime;            // How long between each spawn.
	public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
	public static int counter = 0;		// Counts the amount of the enemys.
	public int limit;					// The limit of the quantity of enemys.
	
	void Start ()
	{
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}
	
	void Spawn ()
	{
		/*
		// If the player has no health left...
		if(playerHealth.currentHealth <= 0f)
		{
			// ... exit the function.
			return;
		}
		*/

		// Find a random index between zero and one less than the number of spawn points.
		int spawnPointIndex = (Random.Range (1, spawnPoints.Length + 1)) - 1;
		
		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		if (counter <= limit){
			counter++;
			//Debug.Log (counter);
			if (counter > 1){
				Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
			}
		} 

	}
}
 