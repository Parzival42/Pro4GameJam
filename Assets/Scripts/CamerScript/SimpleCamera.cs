using UnityEngine;
using System.Collections;

public class SimpleCamera : MonoBehaviour 
{

    Transform followObject;
    public float x_offset = 0;
    public float z_offset = 0;

	// Use this for initialization
	void Start () 
    {
        followObject = GameObject.FindGameObjectsWithTag("Player")[0].transform;
	}
	
	// Update is called once per frame
	void Update () 
    {
        MoveCamera();
	}

    private void MoveCamera()
    {
        transform.position = new Vector3(followObject.transform.position.x + x_offset, transform.position.y, followObject.transform.position.z + z_offset);
    }
}
