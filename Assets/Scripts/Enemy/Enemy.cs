using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float health;
	public string enemyName;

	public void TakeDamage(float damage){
		health -= damage;
		//Debug.Log("Lololo bullet hit!");
		if (health <= 0){
			if (enemyName == "TrashMob"){
				EnemyManager.counterTrashMob--;
			} else if (enemyName == "BigMob") {
				EnemyManager.counterBigMob--;
			}

			Destroy(gameObject);
		}
	}
}
