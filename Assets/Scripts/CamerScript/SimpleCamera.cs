using UnityEngine;
using System.Collections;

public class SimpleCamera : MonoBehaviour 
{
    Transform followObject;
    public float x_offset = 0;
    public float z_offset = 0;

    GameObject[] players;
    Bounds playerBounds;

    private Vector3 velocity = Vector3.zero;
    public float cameraDamping = 1f;
    public float maxSpeed = 1f;

    float lastFrame = 0f;
    float currentFrame = 0f;
    float myDelta = 0f;
    float avg = 0f;

	// Use this for initialization
	void Start () 
    {
        followObject = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        players = GameObject.FindGameObjectsWithTag("Player");
        playerBounds = new Bounds();

        if (players.Length == 0)
            players = null;
	}
	
	// Update is called once per frame
	void Update () 
    {
        CalculateDeltaTime();
        

        if (players != null)
        {
            CalculatePlayerBoundingBox();
        }
	}

    void FixedUpdate()
    {
        
    }

    void LateUpdate()
    {
        MoveCamera();
    }

    /// <summary>
    /// Calculate bounding box over all players.
    /// </summary>
    private void CalculatePlayerBoundingBox()
    {
        float smallestX = players[0].transform.position.x;
        float smallestZ = 0;

        float biggestX = 0;
        float biggestZ = players[0].transform.position.z;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].transform.position.x < smallestX)
                smallestX = players[i].transform.position.x;

            if (players[i].transform.position.z < smallestZ)
                smallestZ = players[i].transform.position.z;

            if (players[i].transform.position.x > biggestX)
                biggestX = players[i].transform.position.x;

            if (players[i].transform.position.z > biggestZ)
                biggestZ = players[i].transform.position.z;

            playerBounds.SetMinMax(new Vector3(smallestX, 0.5f, biggestZ), new Vector3(biggestX, 0.5f, smallestZ));
        }
        //Debug.Log("Min: " + playerBounds.min);
    }

    /// <summary>
    /// Calculate custom delta time
    /// </summary>
    private void CalculateDeltaTime()
    {
        currentFrame = Time.realtimeSinceStartup;
        myDelta = currentFrame - lastFrame;
        lastFrame = currentFrame;
    }

    /// <summary>
    /// Follow the center of the bounding box.
    /// </summary>
    private void MoveCamera()
    {
        avg = (Time.deltaTime + Time.smoothDeltaTime + myDelta) * 0.3333333f;
        Vector3 newPosition = new Vector3(playerBounds.center.x + x_offset, transform.position.y, playerBounds.center.z + z_offset);
        //transform.position = Vector3.Lerp(transform.position, newPosition, avg);

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, cameraDamping, maxSpeed, avg);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(playerBounds.center, playerBounds.size);
    }
}
