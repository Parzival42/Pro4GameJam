﻿using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class SimplePlayer : MonoBehaviour 
{
    string playerPrefix = "P1_";

    public float movementSpeed = 10;
    PlayerManager pManager;

    bool canShoot = true;

    //Tolerance of the analog stick.
    private float analogStickTolerance = 0.1f;
    
    /*
     * True: Player shoots automatically if the right analog stick is moved.
     * False: Player has to shoot manually by pressing the shoot button.
     */
    public bool automaticShoot = false;

    public float timeToShoot = 0.2f;
    public float rotationSpeed = 1f;

    //Determines if the right stick is used or not.
    private bool rightAnalogStickIsUsed = false;

    //LineRenderer
    public float lineLength = 15f;

	//phoneInput
	private StreamReader leftStream = null;
	private StreamReader rightStream = null;

	//phone varialbe values
	private int angleLeft = 0;
	private int distanceLeft = 0;
	private int angleRight = 0;
	private Boolean buttonPressed = false;

	public StreamReader LeftStream {
		set {leftStream = value;}
	}

	public StreamReader RightStream {
		set {rightStream = value;}
	}

	// Use this for initialization
	void Start () 
    {
        LineRenderer line = gameObject.AddComponent<LineRenderer>();
        line.material = new Material(Resources.Load<Material>("LineRendererMaterial"));
        line.SetColors(Color.white, Color.black);
        line.SetWidth(0.1f, 0.1f);
        line.SetVertexCount(2);

        pManager = GameObject.FindObjectOfType<PlayerManager>();
        playerPrefix = pManager.PlayerPrefix;
        name =  pManager.PlayerPrefix + "Player";

        Debug.Log(playerPrefix + "Player added!");
	}
	
	// Update is called once per frame
	void Update () 
    {
        DrawLine();
	}

    void FixedUpdate()
    {
		if (leftStream == null && rightStream == null) {
			HandleInput ();
		} else {
			HandlePhoneInput();
		}

        
    }

    private void HandleInput()
    {
        float leftStickHorizontal = Input.GetAxis(playerPrefix + "Horizontal");
        float leftStickVertical = Input.GetAxis(playerPrefix + "Vertical");

        //Movement======================================================
        if (leftStickHorizontal > analogStickTolerance)
            rigidbody.AddForce(Vector3.right * movementSpeed * Mathf.Abs(leftStickHorizontal));
        else if (leftStickHorizontal < -analogStickTolerance)
            rigidbody.AddForce(-Vector3.right * movementSpeed * Mathf.Abs(leftStickHorizontal));

        if (leftStickVertical > analogStickTolerance)
            rigidbody.AddForce(-Vector3.forward * movementSpeed * Mathf.Abs(leftStickVertical));
        else if (leftStickVertical < -analogStickTolerance)
            rigidbody.AddForce(Vector3.forward * movementSpeed * Mathf.Abs(leftStickVertical));
        //==============================================================

        //Rotation======================================================
        if (Input.GetAxis(playerPrefix + "VerticalRotation") > analogStickTolerance || Input.GetAxis(playerPrefix + "VerticalRotation") < -analogStickTolerance || Input.GetAxis(playerPrefix + "HorizontalRotation") > analogStickTolerance || Input.GetAxis(playerPrefix + "HorizontalRotation") < -analogStickTolerance)
        {
            rightAnalogStickIsUsed = true;
            Vector3 angle = new Vector3(0, Mathf.Atan2(Input.GetAxis(playerPrefix + "HorizontalRotation"), -Input.GetAxis(playerPrefix + "VerticalRotation")) * Mathf.Rad2Deg, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(angle), Time.deltaTime * rotationSpeed);

            if (automaticShoot)
                ShootBullet();
        }

        /*Rotation when automatic shoot is true:
         *  - Orient player to the direction where he is facing (Left analog stick)
         *  - Check if the Right analog stick is used:
         *      - If not -> Face him to the direction
         *      - If yes -> Let the right analog stick do the facing.
         */
        if (!rightAnalogStickIsUsed)
        {
            //Check the analog stick tolerance
            if (leftStickHorizontal > analogStickTolerance || leftStickHorizontal < -analogStickTolerance
                || leftStickVertical > analogStickTolerance || leftStickVertical < -analogStickTolerance)
            {
                Vector3 angle = new Vector3(0, Mathf.Atan2(Input.GetAxis(playerPrefix + "Horizontal"), -Input.GetAxis(playerPrefix + "Vertical")) * Mathf.Rad2Deg, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(angle), Time.deltaTime * rotationSpeed);
            }
        }

        //==============================================================

        //Shoot======================================================
        if (!automaticShoot && Input.GetAxis(playerPrefix + "Shoot") > 0.1)
        {
            ShootBullet();
        }
        //==============================================================

        rightAnalogStickIsUsed = false;
        
    }

	private void HandlePhoneInput()
	{
		float leftStickHorizontal = (float)(Math.Cos (DegreeToRadian (angleLeft)));
		float leftStickVertical = (float)(Math.Sin (DegreeToRadian (angleLeft)));
		float rightStickHorizontal = (float)(Math.Cos (DegreeToRadian (angleRight)));
		float rightStickVertical = (float)(Math.Sin (DegreeToRadian (angleRight)));
		
		//Movement======================================================
		if (leftStickHorizontal > analogStickTolerance)
			rigidbody.AddForce(Vector3.right * movementSpeed * Mathf.Abs(leftStickHorizontal));
		else if (leftStickHorizontal < -analogStickTolerance)
			rigidbody.AddForce(-Vector3.right * movementSpeed * Mathf.Abs(leftStickHorizontal));
		
		if (leftStickVertical > analogStickTolerance)
			rigidbody.AddForce(-Vector3.forward * movementSpeed * Mathf.Abs(leftStickVertical));
		else if (leftStickVertical < -analogStickTolerance)
			rigidbody.AddForce(Vector3.forward * movementSpeed * Mathf.Abs(leftStickVertical));
		//==============================================================
		
		//Rotation======================================================
		if (rightStickVertical > analogStickTolerance || rightStickVertical < -analogStickTolerance || rightStickHorizontal > analogStickTolerance || rightStickHorizontal < -analogStickTolerance)
		{
			rightAnalogStickIsUsed = true;
			Vector3 angle = new Vector3(0, Mathf.Atan2(rightStickHorizontal, -rightStickVertical) * Mathf.Rad2Deg, 0);
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(angle), Time.deltaTime * rotationSpeed);
			
			if (automaticShoot)
				ShootBullet();
		}
		
		/*Rotation when automatic shoot is true:
         *  - Orient player to the direction where he is facing (Left analog stick)
         *  - Check if the Right analog stick is used:
         *      - If not -> Face him to the direction
         *      - If yes -> Let the right analog stick do the facing.
         */
		if (!rightAnalogStickIsUsed)
		{
			//Check the analog stick tolerance
			if (leftStickHorizontal > analogStickTolerance || leftStickHorizontal < -analogStickTolerance
			    || leftStickVertical > analogStickTolerance || leftStickVertical < -analogStickTolerance)
			{
				Vector3 angle = new Vector3(0, Mathf.Atan2(Input.GetAxis(playerPrefix + "Horizontal"), -Input.GetAxis(playerPrefix + "Vertical")) * Mathf.Rad2Deg, 0);
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(angle), Time.deltaTime * rotationSpeed);
			}
		}
		
		//==============================================================
		
		//Shoot======================================================
		if (!automaticShoot && Input.GetAxis(playerPrefix + "Shoot") > 0.1)
		{
			ShootBullet();
		}
		//==============================================================
		
		rightAnalogStickIsUsed = false;
		
	}

	public void UpdateVariables() {

		//Listen to left stick

		UnityThreadHelper.CreateThread(() =>
		                               {

			while (true) {
				
				String data = leftStream.ReadLine();
				String[] tokens = data.Split(' ');
				
				if (tokens.Length == 2) {
					if (tokens[0] != "") {
						angleLeft = int.Parse(tokens[0].Substring(1));
					}
					
					if (tokens[1] != "") {
						distanceLeft = int.Parse(tokens[1].Substring(1));
					}
					
				} else if (tokens.Length == 1) {
					
					if (tokens[0] != "") {
						if (tokens[0].Substring(0,1).Equals("0")) {
							angleLeft = int.Parse(tokens[0].Substring(1));
						}
						
						if (tokens[0].Substring(0,1).Equals("1")) {
							distanceLeft = int.Parse(tokens[0].Substring(1));
						}
					}
				}
				
			};
			
		});

		//Listen to right stick

		UnityThreadHelper.CreateThread(() =>
		                               {

			while (true) {
				
				String data = rightStream.ReadLine();
				
				if (data.Substring(0,1).Equals("a")) {
					if (data.Substring(1,2).Equals("0")) {
						buttonPressed = false;
					} else {
						buttonPressed = true;
					}
				} else {
					angleRight = int.Parse(data);
				}
				
			};
			
		});

	}

    /// <summary>
    /// Shoots the bullet. A new bullet instance will be created.
    /// </summary>
    private void ShootBullet()
    {
        if (canShoot)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Bullet")) as GameObject;

            BulletScript bullet = obj.GetComponent<BulletScript>();
            bullet.Direction = transform.forward;
            bullet.transform.position = transform.FindChild("BulletSpawnPoint").transform.position;

            bullet.name = "Bullet";

            canShoot = false;
            StartCoroutine(WaitForShoot());

            //TODO: implement functionality in Bullet class
            Destroy(obj, bullet.lifeTime);
        }
    }

    private void DrawLine()
    {
        LineRenderer line = GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + transform.forward * lineLength);
    }

    IEnumerator WaitForShoot()
    {
        yield return new WaitForSeconds(timeToShoot);
        canShoot = true;
    }

	private double DegreeToRadian(int angle) {
		return (270.0 - (Math.PI * (double)angle / 180.0));
	}
}
