using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectiles : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;

    public FMODUnity.EventReference ArrowWhoosh_ER;
    public void LaunchProjectile()
    {
        if( projectile == null)
        {
            Debug.LogWarning("Pas de Projectiles dans la chambre", this);
            return;
        }
        Instantiate(projectile, transform.position, transform.rotation);
        FMODUnity.RuntimeManager.PlayOneShot(ArrowWhoosh_ER, transform.position);
    }
}
