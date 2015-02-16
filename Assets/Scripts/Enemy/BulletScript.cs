using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	public float damage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider collider){
		Debug.Log("Lololo bullet hit!");
		if (collider.gameObject.tag == "Enemy"){
			collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
		}
	}
}
