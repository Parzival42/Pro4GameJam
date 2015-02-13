using UnityEngine;
using System.Collections;

/// <summary>
/// If this script is activated, the gameobject can be moved in a specific step size.
/// </summary>
[ExecuteInEditMode]
public class EditorGrid : MonoBehaviour
{
    public float cellSizeX = 1f;
    public float cellSizeY = 1f;
    public float cellSizeZ = 1f;

    private float x, y, z;

    void Start()
    {
        GetComponent<EditorGrid>().enabled = true;
        x = 0f;
        y = 0f;
        z = 0f;

        if (Application.isPlaying)
            GetComponent<EditorGrid>().enabled = false;
    }

    void Update()
    {
        x = Mathf.Round(transform.position.x / cellSizeX) * cellSizeX;
        y = Mathf.Round(transform.position.y / cellSizeY) * cellSizeY;
        z = Mathf.Round(transform.position.y / cellSizeZ) * cellSizeZ;
        transform.position = new Vector3(x, y, z);
    }
}