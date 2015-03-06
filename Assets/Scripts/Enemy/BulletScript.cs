using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour, IBullet
{
	public float damage = 50;
    public float speed = 10;
    public float lifeTime = 5f;
    private bool shootEnabled = false;

    protected Vector3 direction;

    public Vector3 Direction
    {
        get { return direction; }
        set { this.direction = value; }
    }

	// Use this for initialization
	protected virtual void Start () 
    {
        
	}
	
	// Update is called once per frame
    protected virtual void Update() 
    {
        if(shootEnabled)
            MoveBullet();
	}

    public virtual void Shoot()
    {
        Debug.Log("BulletScript: Shoot");
        shootEnabled = true;
        StartCoroutine(WaitForDestruction());
    }

    protected virtual void MoveBullet()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
		//Debug.Log("Lololo bullet hit!");
		if (collider.gameObject.tag == "Enemy")
        {
			Destroy(gameObject);
			collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            
		}
	}

    IEnumerator WaitForDestruction()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
