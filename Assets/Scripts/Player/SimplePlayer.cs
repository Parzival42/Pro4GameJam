using UnityEngine;
using System.Collections;

public class SimplePlayer : MonoBehaviour 
{
    string playerPrefix = "P1_";

    public float movementSpeed = 10;
    PlayerManager pManager;

    bool canShoot = true;
    public float timeToShoot = 0.2f;

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

        //Rotation
        if (Input.GetAxis(playerPrefix + "VerticalRotation") > 0.1 || Input.GetAxis(playerPrefix + "VerticalRotation") < -0.1 || Input.GetAxis(playerPrefix + "HorizontalRotation") > 0.1 || Input.GetAxis(playerPrefix + "HorizontalRotation") < -0.1)
        {
            Vector3 angle = new Vector3(0, Mathf.Atan2(Input.GetAxis(playerPrefix + "HorizontalRotation"), -Input.GetAxis(playerPrefix + "VerticalRotation")) * Mathf.Rad2Deg, 0);
            transform.rotation = Quaternion.Euler(angle);
        }

        //Shoot
        if (Input.GetAxis(playerPrefix + "Shoot") > 0.1 && canShoot)
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

    IEnumerator WaitForShoot()
    {
        yield return new WaitForSeconds(timeToShoot);
        canShoot = true;
    }
}
