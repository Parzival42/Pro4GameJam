using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float health;
	private Transform enemy;

	public void TakeDamage(float damage){
		health -= damage;
		//Debug.Log("Lololo bullet hit!");
		if (health <= 0){
			EnemyManager.counter--;
			Destroy(gameObject);
		}
	}
}
