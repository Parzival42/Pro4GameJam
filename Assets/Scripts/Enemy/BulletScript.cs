using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour 
{
	public float damage = 50;
    public float speed = 10;
    public float lifeTime = 5f;


    private Vector3 direction;

    public Vector3 Direction
    {
        get { return direction; }
        set { this.direction = value; }
    }

	// Use this for initialization
	void Start () 
    {
        //StartCoroutine(WaitForDestruction());
	}
	
	// Update is called once per frame
	void Update () 
    {
        MoveBullet();
	}

    private void MoveBullet()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

	void OnTriggerEnter(Collider collider)
    {
		Debug.Log("Lololo bullet hit!");
		if (collider.gameObject.tag == "Enemy")
        {
			Destroy(gameObject);
			collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            
		}
	}

    //IEnumerator WaitForDestruction()
    //{
    //    yield return new WaitForSeconds(lifeTime);
    //    Destroy(this);
    //}
}
