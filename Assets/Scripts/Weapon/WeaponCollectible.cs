using UnityEngine;
using System.Collections;

public class WeaponCollectible : MonoBehaviour 
{
    public string bulletPrefabName = "Bullet";
    public float rotationSpeed = 2.5f;

    public enum WeaponType { StandardWeapon, FastWeapon};
    public WeaponType weaponType;

    private StandardWeapon weapon;

    void Start()
    {
        switch (weaponType)
        {
            case WeaponType.StandardWeapon:
                weapon = new StandardWeapon();
                weapon.BulletPrefabName = bulletPrefabName;
                break;
            case WeaponType.FastWeapon:
                weapon = new StandardWeapon();
                weapon.BulletPrefabName = bulletPrefabName;
                break;
        }
    }

	// Update is called once per frame
	void Update () 
    {
        transform.Rotate(new Vector3(12, 33, 55) * Time.deltaTime * rotationSpeed);
	}

    /// <summary>
    /// Give the player the weapon and destroy the weapon collectible gameobject.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !other.isTrigger)
        {
            Debug.Log("Player collided with Weapon");
            SimplePlayer p = other.gameObject.GetComponent<SimplePlayer>();
            p.Weapon = weapon;

            Destroy(gameObject, 0.1f);
        }
    }
}
