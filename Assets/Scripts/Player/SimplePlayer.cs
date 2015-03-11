using UnityEngine;
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

    //Line Renderer
    public float lineLength = 15f;

    //Weapon
    StandardWeapon weapon;
    public StandardWeapon Weapon
    {
        get { return weapon; }
        set { weapon = value; }
    }


	//tells the number of phone players currently involved
	private int phonePlayer;

	//tells wether the player is a phone or not
	private Boolean isPhone = false;
	
	// acces the network data
	private PlayerNetCommunicate playerComm;

	public int PhonePlayer {
		get {return phonePlayer;}
		set {phonePlayer = value;}
	}

	public Boolean IsPhone {
		set {isPhone = value;}
	}

	// Use this for initialization
	void Start () 
    {
		playerComm = GameObject.FindObjectOfType<PlayerNetCommunicate>();

        LineRenderer line = gameObject.AddComponent<LineRenderer>();
        line.material = new Material(Resources.Load<Material>("LineRendererMaterial"));
        line.SetColors(Color.white, Color.black);
        line.SetWidth(0.1f, 0.1f);
        line.SetVertexCount(2);

        pManager = GameObject.FindObjectOfType<PlayerManager>();
        playerPrefix = pManager.PlayerPrefix;
        name =  pManager.PlayerPrefix + "Player";

        weapon = null;

        Debug.Log(playerPrefix + "Player added!");

	}

	// Update is called once per frame
	void Update () 
    {
        DrawLine();
	}

    void FixedUpdate()
    {

		if (isPhone) {
			HandlePhoneInput();
		} else {
			HandleInput ();
		}
        
    }

    private void HandleInput()
    {
        float leftStickHorizontal = Input.GetAxis(playerPrefix + "Horizontal");
        float leftStickVertical = Input.GetAxis(playerPrefix + "Vertical");

        //Movement======================================================
        if (leftStickHorizontal > analogStickTolerance)
            GetComponent<Rigidbody>().AddForce(Vector3.right * movementSpeed * Mathf.Abs(leftStickHorizontal));
        else if (leftStickHorizontal < -analogStickTolerance)
            GetComponent<Rigidbody>().AddForce(-Vector3.right * movementSpeed * Mathf.Abs(leftStickHorizontal));

        if (leftStickVertical > analogStickTolerance)
            GetComponent<Rigidbody>().AddForce(-Vector3.forward * movementSpeed * Mathf.Abs(leftStickVertical));
        else if (leftStickVertical < -analogStickTolerance)
            GetComponent<Rigidbody>().AddForce(Vector3.forward * movementSpeed * Mathf.Abs(leftStickVertical));
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

		float leftStickHorizontal = (float) (Math.Cos (DegreeToRadian (playerComm.angleLeft[phonePlayer])) * playerComm.distanceLeft[phonePlayer] / playerComm.stickSize[phonePlayer]);
		float leftStickVertical = (float)(Math.Sin (DegreeToRadian (playerComm.angleLeft[phonePlayer])) * playerComm.distanceLeft[phonePlayer] / playerComm.stickSize[phonePlayer]);
		float rightStickHorizontal = (float)(Math.Cos (DegreeToRadian (playerComm.angleRight[phonePlayer])) * playerComm.distanceRight[phonePlayer] / playerComm.stickSize[phonePlayer]);
		float rightStickVertical = (float)(Math.Sin (DegreeToRadian (playerComm.angleRight[phonePlayer])) * playerComm.distanceRight[phonePlayer] / playerComm.stickSize[phonePlayer]);
		
		//Movement======================================================
		if (leftStickHorizontal > analogStickTolerance)
			GetComponent<Rigidbody>().AddForce(Vector3.right * movementSpeed * Mathf.Abs(leftStickHorizontal));
		else if (leftStickHorizontal < -analogStickTolerance)
			GetComponent<Rigidbody>().AddForce(-Vector3.right * movementSpeed * Mathf.Abs(leftStickHorizontal));
		
		if (leftStickVertical > analogStickTolerance)
			GetComponent<Rigidbody>().AddForce(-Vector3.forward * movementSpeed * Mathf.Abs(leftStickVertical));
		else if (leftStickVertical < -analogStickTolerance)
			GetComponent<Rigidbody>().AddForce(Vector3.forward * movementSpeed * Mathf.Abs(leftStickVertical));
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
				Vector3 angle = new Vector3(0, Mathf.Atan2(rightStickHorizontal, -rightStickVertical) * Mathf.Rad2Deg, 0);
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(angle), Time.deltaTime * rotationSpeed);
			}
		}
		
		//==============================================================
		
		//Shoot======================================================
		if (!automaticShoot && playerComm.buttonPressed[phonePlayer] == 1)
		{
			ShootBullet();
		}
		//==============================================================
		
		rightAnalogStickIsUsed = false;
		
	}

    /// <summary>
    /// Shoots the bullet. A new bullet instance will be created.
    /// </summary>
    private void ShootBullet()
    {
        Debug.Log("Weapon: " + weapon);
        if (weapon != null && canShoot)
        {
            Debug.Log("Instantiate: " + weapon.BulletPrefabName);
            GameObject obj = Instantiate(Resources.Load<GameObject>(weapon.BulletPrefabName)) as GameObject;

            BulletScript bullet = obj.GetComponent<BulletScript>(); ;
            
            bullet.Direction = transform.forward;
            bullet.transform.position = transform.FindChild("BulletSpawnPoint").transform.position;
            bullet.name = "Bullet";
            (bullet as IBullet).Shoot();


            canShoot = false;
            StartCoroutine(WaitForShoot());
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

	private double DegreeToRadian(int angle) 
    {
		return ((Math.PI * (double)angle / 180.0));
	}
}
