using UnityEngine;
using System.Collections;

public class SimplePlayer : MonoBehaviour 
{
    string playerPrefix = "P1_";

    public float movementSpeed = 10;
    PlayerManager pManager;

	// Use this for initialization
	void Start () 
    {
        pManager = GameObject.FindObjectOfType<PlayerManager>();
        playerPrefix = pManager.PlayerPrefix;
        name =  pManager.PlayerPrefix + "Player";

        Debug.Log(playerPrefix + "Player added!");
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    void FixedUpdate()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        

        if (Input.GetAxis(playerPrefix + "Horizontal") > 0.1)
        {
            rigidbody.AddForce(Vector3.right * movementSpeed);
        }
        else if (Input.GetAxis(playerPrefix + "Horizontal") < -0.1)
            rigidbody.AddForce(-Vector3.right * movementSpeed);

        if (Input.GetAxis(playerPrefix + "Vertical") > 0.1)
            rigidbody.AddForce(-Vector3.forward * movementSpeed);
        else if (Input.GetAxis(playerPrefix + "Vertical") < -0.1)
            rigidbody.AddForce(Vector3.forward * movementSpeed);

    }
}
