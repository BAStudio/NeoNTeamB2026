using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Pistol : WeaponBase
{
    [SerializeField] private GameObject pistolTip;
    protected override void Shoot()
    {
        Vector3 direction = GetAccuracyDirection();
        Camera cam = Camera.main;

        if (muzzleFlashPrefab != null)
        {
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, pistolTip.transform.position, Quaternion.LookRotation(direction) * Quaternion.Euler(180f, 0f, 0f));
            Destroy(muzzleFlash, 3f);
        }

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, direction, out hit))
        {
            if (hit.collider.tag != "Enemy")
            {
                Vector3 endPosition = hit.point;
                GameObject hole = Instantiate(bulletHolePrefab, endPosition + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal));
                GameObject smoke = Instantiate(bulletSmoke, endPosition, Quaternion.Euler(180f, 0f, 0f));
                ParticleSystem ps = smoke.GetComponent<ParticleSystem>();
                var main = ps.main;
                main.loop = false;
                main.stopAction = ParticleSystemStopAction.Destroy;
                ps.Play();
                Destroy(hole, 5f);
            }
            Debug.Log("Hit: " + hit.collider.name);
        }

        currentAmmo--;
        Debug.Log("ammo: " + currentAmmo);

        StartCoroutine(DelayedScreenShake());
    }

    private IEnumerator DelayedScreenShake()
    {
        yield return new WaitForSeconds(0.05f);
        ScreenShake();
    }
}