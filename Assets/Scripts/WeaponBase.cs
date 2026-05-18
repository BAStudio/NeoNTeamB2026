using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected int maxAmmo = 12;
    [SerializeField] protected int currentAmmo = 12;
    [SerializeField] protected float shootCooldown = 0.5f;
    [SerializeField] protected float reloadTime = 1.5f;
    [SerializeField] protected float accuracy = 0.5f;
    [SerializeField] protected float aimAccuracy = 1f;
    //[SerializeField] protected GameObject bulletHolePrefab;
    [SerializeField] protected GameObject muzzleFlashPrefab;
    [SerializeField] protected bool shootUseMouseButtonDown = true;
    public float Accuracy => accuracy;
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;
    protected float OriginalAccuracy;
    protected float lastShootTime = 0f;
    protected bool isReloading = false;
    protected float reloadCooldown = 0f;
    [SerializeField] protected ItemData ammoItemData;
    // Sleep hier je Ammo ItemData in bij de pistol
    [Header("Audio")]
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioSource audioSource;

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

    protected abstract void Shoot();
    protected virtual void StartReload()
    {
        if (ammoItemData == null)
        {
            Debug.LogError("Geen Ammo ItemData gekoppeld aan dit wapen.");
            return;
        }

        if (InventorySystem.Instance == null)
        {
            Debug.LogError("Geen InventorySystem gevonden.");
            return;
        }

        bool hasAmmo = InventorySystem.Instance.HasItem(ammoItemData.itemName);
        // Checkt of Ammo in je inventory/hotbar zit

        if (!hasAmmo)
        {
            Debug.Log("Geen ammo in inventory.");
            return;
        }

        Debug.Log("Reloading...");
        isReloading = true;
        reloadCooldown = reloadTime;
    }
    protected virtual void CompleteReload()
    {
        if (ammoItemData == null)
        {
            Debug.LogError("Geen Ammo ItemData gekoppeld.");
            isReloading = false;
            return;
        }

        bool removedAmmo = InventorySystem.Instance.RemoveOneItemByName(ammoItemData.itemName);
        // Verbruikt 1 Ammo item uit je inventory

        if (!removedAmmo)
        {
            Debug.Log("Reload mislukt: geen ammo meer.");
            isReloading = false;
            return;
        }

        currentAmmo = maxAmmo;
        // Pistool is weer vol geladen

        isReloading = false;

        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        Debug.Log("Reload compleet. Ammo verbruikt: " + ammoItemData.itemName);
    }
}
