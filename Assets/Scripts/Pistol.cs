using UnityEngine;

public class Pistol : WeaponBase
{
    protected override void Shoot()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("geen ammo");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.Log("Hit: " + hit.collider.name);
        }

        currentAmmo--;
        Debug.Log("ammo: " + currentAmmo);
    }
}