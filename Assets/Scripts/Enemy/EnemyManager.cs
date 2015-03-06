using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	//public PlayerHealth playerHealth;       	// Reference to the player's heatlh.
	public bool spawnTrashMob;
	public GameObject trashMob;                	// The enemy prefab to be spawned.
	public float spawnTimeTrashMob;            	// How long between each spawn.
	public float spawnStartTrashMob;			// Time after the enemy starts spawning.
	public Transform[] spawnPointsTrashMob;     // An array of the spawn points this enemy can spawn from.
	public static int counterTrashMob = 0;		// Counts the amount of the enemys.
	public int limitTrashMob;					// The limit of the quantity of enemys.

	//______________________________________

	public bool spawnBigMob;
	public GameObject bigMob;                	// The enemy prefab to be spawned.
	public float spawnTimeBigMob;            	// How long between each spawn.
	public float spawnStartBigMob;				// Time after the enemy starts spawning.
	public Transform[] spawnPointsBigMob;    	// An array of the spawn points this enemy can spawn from.
	public static int counterBigMob = 0;		// Counts the amount of the enemys.
	public int limitBigMob;						// The limit of the quantity of enemys.
	

	void Start ()
	{
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		InvokeRepeating ("SpawnTrashMob", spawnStartTrashMob, spawnTimeTrashMob);
		InvokeRepeating ("SpawnBigMob", spawnStartBigMob, spawnTimeBigMob);
	}

	void SpawnTrashMob(){
		Spawn (trashMob, spawnPointsTrashMob);
	}

	void SpawnBigMob(){
		Spawn (bigMob, spawnPointsBigMob);
	}

	void Spawn (GameObject enemy, Transform[] spawnPoints)
	{
		// Find a random index between zero and one less than the number of spawn points.
		int spawnPointIndex = (Random.Range (1, spawnPoints.Length + 1)) - 1;
		if (enemy == trashMob){
			if (counterTrashMob <= limitTrashMob && spawnTrashMob){
				counterTrashMob++;
				//Debug.Log (counter);
				if (counterTrashMob > 1){
					// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
					Instantiate (trashMob, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
				}			
			} 
		} else if(enemy == bigMob) {
			if (counterBigMob <= limitBigMob && spawnBigMob){
				counterBigMob++;
				//Debug.Log (counter);
				if (counterBigMob > 1){
					// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
					Instantiate (bigMob, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
				}			
			} 
		}
	}
}
 