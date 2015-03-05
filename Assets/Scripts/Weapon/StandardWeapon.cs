using UnityEngine;
using System.Collections;

public class StandardWeapon
{
    private string bulletPrefabName;

    /// <summary>
    /// Gets or sets the bulletprefabname
    /// </summary>
    public string BulletPrefabName
    {
        get { return bulletPrefabName; }
        set { bulletPrefabName = value; }
    }
}
