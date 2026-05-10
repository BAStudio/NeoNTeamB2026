using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Pistol : WeaponBase
{
    [SerializeField] private GameObject pistolTip;
    [SerializeField] private int damage = 10;
    //[SerializeField] private GameObject bloodVFXPrefab;
    protected override void Shoot()
    {
        Vector3 direction = GetAccuracyDirection();
        Vector3 raycastOrigin = Camera.main.transform.position;

        if (muzzleFlashPrefab != null)
        {
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, pistolTip.transform.position, Quaternion.LookRotation(direction) * Quaternion.Euler(180f, 0f, 0f));
            Destroy(muzzleFlash, 3f);
        }

        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin, direction, out hit))
        {
            if (hit.collider.tag == "Enemy")
            {
                EnemyMovingController enemy = hit.collider.GetComponent<EnemyMovingController>();
                if (enemy != null)
                {
                    enemy.health -= damage;
                    Debug.Log("Enemy hit! Health: " + enemy.health);
                    if (enemy.health <= 0)
                    {
                        Destroy(enemy.gameObject);
                        Debug.Log("Enemy destroyed!");
                    }
                }
                //if (bloodVFXPrefab != null)
                //{
                //    GameObject blood = Instantiate(bloodVFXPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                //    Destroy(blood, 3f);
                //}
            }
            else
            {
                Vector3 endPosition = hit.point;
                //GameObject hole = Instantiate(bulletHolePrefab, endPosition + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal));
                //GameObject smoke = Instantiate(bulletSmoke.gameObject, endPosition, Quaternion.Euler(180f, 0f, 0f));
                //ParticleSystem ps = smoke.GetComponent<ParticleSystem>();
                //var main = ps.main;
                //main.loop = false;
                //main.stopAction = ParticleSystemStopAction.Destroy;
                //ps.Play();
                //Destroy(hole, 5f);
            }
            Debug.Log("Hit: " + hit.collider.name);
        }

        currentAmmo--;
        Debug.Log("ammo: " + currentAmmo);
    }
}