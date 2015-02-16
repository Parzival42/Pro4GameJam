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
        
        //Movement
        if (Input.GetAxis(playerPrefix + "Horizontal") > 0.1)
            rigidbody.AddForce(Vector3.right * movementSpeed);
        else if (Input.GetAxis(playerPrefix + "Horizontal") < -0.1)
            rigidbody.AddForce(-Vector3.right * movementSpeed);

        if (Input.GetAxis(playerPrefix + "Vertical") > 0.1)
            rigidbody.AddForce(-Vector3.forward * movementSpeed);
        else if (Input.GetAxis(playerPrefix + "Vertical") < -0.1)
            rigidbody.AddForce(Vector3.forward * movementSpeed);

        if (Input.GetAxis(playerPrefix + "VerticalRotation") > 0.1 || Input.GetAxis(playerPrefix + "VerticalRotation") < -0.1 || Input.GetAxis(playerPrefix + "HorizontalRotation") > 0.1 || Input.GetAxis(playerPrefix + "HorizontalRotation") < -0.1)
        {
            Vector3 angle = new Vector3(0, Mathf.Atan2(Input.GetAxis(playerPrefix + "HorizontalRotation"), -Input.GetAxis(playerPrefix + "VerticalRotation")) * Mathf.Rad2Deg, 0);
            transform.rotation = Quaternion.Euler(angle);
        }

        //Rotation


        //if (Input.GetAxis(playerPrefix + "HorizontalRotation") > 0.1)
        //{
            
        //}
        //else if (Input.GetAxis(playerPrefix + "HorizontalRotation") < -0.1)
        //{
        //    deltaRot = Quaternion.Euler(rigidbody.rotation * -angle);
        //    rigidbody.MoveRotation(deltaRot);
        //}

    }
}
