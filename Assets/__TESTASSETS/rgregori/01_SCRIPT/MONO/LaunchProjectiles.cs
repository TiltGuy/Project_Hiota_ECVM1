using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectiles : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;
    public void LaunchProjectile()
    {
        if( projectile == null)
        {
            Debug.LogWarning("Pas de Projectiles dans la chambre", this);
            return;
        }
        Instantiate(projectile, transform.position, Quaternion.identity);
    }
}
