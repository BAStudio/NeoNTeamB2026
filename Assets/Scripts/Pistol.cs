using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Pistol : WeaponBase
{
    protected override void Shoot()
    {
        Vector3 direction = GetAccuracyDirection();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            Vector3 endPosition = hit.point;
            StartCoroutine(SpawnHitParticle(hit));
            Debug.Log("Hit: " + hit.collider.name);
        }

        currentAmmo--;
        Debug.Log("ammo: " + currentAmmo);
    }

    private IEnumerator SpawnHitParticle(RaycastHit hit)
    {
        ParticleSystem particle = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.normal));
        yield return new WaitForSeconds(1f);
        Destroy(particle.gameObject);
    }
}