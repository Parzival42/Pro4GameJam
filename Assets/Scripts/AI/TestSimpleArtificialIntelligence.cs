using UnityEngine;
using System.Collections;

public class TestSimpleArtificialIntelligence : MonoBehaviour {

	private float distance;
	public Transform target;
	public float lookAtDistance;
	public float attackRange;
	public float moveSpeed;
	public float rotationSpeed;

	// Update is called once per frame
	void Update () {

		distance = Vector3.Distance(target.position , transform.position);

		if (distance < lookAtDistance){
			GetComponent<Renderer>().material.color = Color.yellow;
			LookAt();
		}

		if (distance >= lookAtDistance){
			GetComponent<Renderer>().material.color = Color.green;
		}
		if (distance < attackRange){
			GetComponent<Renderer>().material.color = Color.red;
			Attack();
		}
	}

	void LookAt(){
		var rotation = Quaternion.LookRotation(target.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
	}


	void Attack(){
		transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
	}

}
