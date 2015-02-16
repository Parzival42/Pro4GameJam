using UnityEngine;
using System.Collections;

public class TestPlayer : MonoBehaviour 
{
	public float movementspeed;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		HandleInput ();
	}

	void HandleInput() 
	{
		if (Input.GetAxis ("Horizontal") > 0) {
			rigidbody.AddForce(Vector3.right*movementspeed);
		} else if(Input.GetAxis ("Horizontal") < 0) {
			this.rigidbody.AddForce(Vector3.left*movementspeed);
		}

		if (Input.GetAxis ("Vertical") > 0) {
			this.rigidbody.AddForce(Vector3.forward*movementspeed);
		} else if(Input.GetAxis ("Vertical") < 0) {
			this.rigidbody.AddForce(-Vector3.forward*movementspeed);
		}

		if (Input.GetAxis ("Jump") > 0) {
			this.rigidbody.AddForce(Vector3.up*50);
			Debug.Log("PlayerJump");
		}
	}
}
