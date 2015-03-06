using UnityEngine;
using System.Collections;

public class LineBullet : BulletScript, IBullet
{
    
    public float maxLineLength = 20f;

    void Awake()
    {
        LineRenderer bulletLine;

        GetComponent<MeshRenderer>().enabled = false;

        bulletLine = gameObject.AddComponent<LineRenderer>();
        bulletLine.material = new Material(Resources.Load<Material>("LineBulletMaterial"));
        bulletLine.SetColors(Color.white, Color.black);
        bulletLine.SetWidth(0.1f, 0.1f);
        bulletLine.SetVertexCount(2);
    }

    protected virtual void Start()
    {
        base.Start();

        
    }


    protected virtual void Update()
    {
        
    }

    /// <summary>
    /// Casts a Ray and checks if there is an enemy to hit.
    /// </summary>
    protected virtual void Shoot()
    {
        RaycastHit hitInfo;
        LineRenderer bulletLine = GetComponent<LineRenderer>();

        Debug.Log("LineBullet: Shoot");

        //Check if it hit something.
        if (Physics.Raycast(transform.position, Direction, out hitInfo, maxLineLength))
        {
            Debug.Log("LineBullet: Hit");
            bulletLine.SetPosition(0, transform.position);
            bulletLine.SetPosition(1, transform.position + (Direction * Vector3.Distance(transform.position, hitInfo.transform.position)));

            if (hitInfo.transform.gameObject.GetComponent<Enemy>() != null)
            {
                hitInfo.transform.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log("LineBullet: Miss");
            bulletLine.SetPosition(0, transform.position);
            bulletLine.SetPosition(1, transform.position + Direction * maxLineLength);
        }

        Destroy(gameObject, 1f);
    }

    protected virtual void MoveBullet()
    {
 
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {

    }
}
