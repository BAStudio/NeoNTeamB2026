using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected int maxAmmo = 12;
    [SerializeField] protected int currentAmmo = 12;
    [SerializeField] protected float shootCooldown = 0.5f;
    [SerializeField] protected float reloadTime = 1.5f;
    protected float lastShootTime = 0f;
    protected bool isReloading = false;
    protected float reloadCooldown = 0f;
    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastShootTime && !isReloading && currentAmmo > 0)
        {
            lastShootTime = Time.time + shootCooldown;
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo)
        {
            StartReload();
        }
        if (isReloading)
        {
            reloadCooldown -= Time.deltaTime;
            if (reloadCooldown <= 0f)
            {
                CompleteReload();
            }
        }
    }
    protected abstract void Shoot();
    protected virtual void StartReload()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log("reloading");
#endif
        isReloading = true;
        reloadCooldown = reloadTime;
    }
    protected virtual void CompleteReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 10f);
    }
}
