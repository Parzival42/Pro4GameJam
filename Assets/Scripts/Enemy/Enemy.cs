using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	private float health;
	private Transform enemy;

	void Start () {
		health = 100.00f;
	}

	public void TakeDamage(float damage){
		health -= damage;
		Debug.Log("Lololo bullet hit!");
		if (health <= 0){
			Destroy(gameObject);
		}
	}
}
