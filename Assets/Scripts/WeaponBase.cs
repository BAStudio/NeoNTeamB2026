using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected int maxAmmo = 12;
    [SerializeField] protected int currentAmmo = 12;
    [SerializeField] protected float shootCooldown = 0.5f;
    [SerializeField] protected float reloadTime = 1.5f;
    [SerializeField] protected float accuracy = 0.5f;
    [SerializeField] protected float aimAccuracy = 1f;
    [SerializeField] protected GameObject bulletHolePrefab;
    [SerializeField] protected GameObject muzzleFlashPrefab;
    [SerializeField] protected bool shootUseMouseButtonDown = true;
    [SerializeField] protected GameObject bulletSmoke;
    [SerializeField] protected float screenshakeMagnitude = 0.2f;
    [SerializeField] protected float screenshakeDuration = 0.1f;
    public float Accuracy => accuracy;
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;
    public bool IsReloading => isReloading;
    protected float OriginalAccuracy;
    protected float lastShootTime = 0f;
    protected bool isReloading = false;
    protected float reloadCooldown = 0f;

    protected virtual void Start()
    {
        OriginalAccuracy = accuracy;
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButton(1))
        {
            accuracy = Mathf.Lerp(accuracy, aimAccuracy, Time.deltaTime * 5f);
        }
        else
        {
            accuracy = Mathf.Lerp(accuracy, OriginalAccuracy, Time.deltaTime * 5f);
        }

        bool shootInput = shootUseMouseButtonDown ? Input.GetMouseButtonDown(0) : Input.GetMouseButton(0);
        if (shootInput && Time.time >= lastShootTime && !isReloading && currentAmmo > 0)
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

    protected Vector3 GetAccuracyDirection()
    {
        Camera cam = Camera.main;
        Ray centerRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;
        if (Physics.Raycast(centerRay, out RaycastHit hit, 1000f))
        {
            aimPoint = hit.point;
        }
        else
        {
            aimPoint = centerRay.GetPoint(1000f);
        }

        Vector3 direction = (aimPoint - cam.transform.position).normalized;

        float spread = 1 - accuracy;
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * spread;
        direction += cam.transform.right * randomCircle.x + cam.transform.up * randomCircle.y;
        return direction.normalized;
    }

    protected void ScreenShake()
    {
        StartCoroutine(ScreenShakeCoroutine());
    }

    protected IEnumerator ScreenShakeCoroutine()
    {
        Camera cam = Camera.main;
        Vector3 originalPos = cam.transform.position;
        float elapsed = 0f;

        while (elapsed < screenshakeDuration)
        {
            float x = Random.Range(-1f, 1f) * screenshakeMagnitude;
            float y = Random.Range(-1f, 1f) * screenshakeMagnitude;
            cam.transform.position = originalPos + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = originalPos;
    }

    protected abstract void Shoot();
    protected virtual void StartReload()
    {
        Debug.Log("reloading");
        isReloading = true;
        reloadCooldown = reloadTime;
    }
    protected virtual void CompleteReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
