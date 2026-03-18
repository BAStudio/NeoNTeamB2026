using UnityEngine;

public class AssaultRifle : WeaponBase
{

    protected override void Shoot()
    {
        Vector3 direction = GetAccuracyDirection();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            if (hit.collider.tag != "Enemy")
            {
                Vector3 endPosition = hit.point;
                GameObject hole = Instantiate(bulletHolePrefab, endPosition + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal));
                //GameObject smoke = Instantiate(bulletSmoke.gameObject, endPosition, Quaternion.Euler(180f, 0f, 0f));
               // ParticleSystem ps = smoke.GetComponent<ParticleSystem>();
                //var main = ps.main;
                //main.loop = false;
                //main.stopAction = ParticleSystemStopAction.Destroy;
                //ps.Play();
                Destroy(hole, 5f);
            }
            Debug.Log("Hit: " + hit.collider.name);
        }

        currentAmmo--;
        Debug.Log("ammo: " + currentAmmo);
    }
}
