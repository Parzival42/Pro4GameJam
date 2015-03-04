using UnityEngine;
using System.Collections;

public class SimpleAIAgent : MonoBehaviour {
	public Transform player;
	private NavMeshAgent agent;

	private float distance = 0.0f;
	public int lookAtDistance;
	public int attackRange;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();

	}
	
	// Update is called once per frame
	void Update () {

		distance = Vector3.Distance(player.position , transform.position);
		
		
		if (distance < lookAtDistance){
			GetComponent<Renderer>().material.color = Color.yellow;
			agent.Stop();
		}
		
		if (distance >= lookAtDistance){
			GetComponent<Renderer>().material.color = Color.green;
			agent.Stop();
		}
		
		if (distance < attackRange){
			GetComponent<Renderer>().material.color = Color.red;
			agent.SetDestination(player.position);
		}

	}
	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.tag == "Player"){
			player = collider.gameObject.GetComponent<Transform>();
			agent.SetDestination(player.position);
		}
	}
}
