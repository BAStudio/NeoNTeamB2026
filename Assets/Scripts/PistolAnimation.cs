using UnityEngine;

public class PistolAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private WeaponBase weapon;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject axe;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (weapon == null)
        {
            weapon = GetComponent<WeaponBase>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null)
        {
            if (pistol != null && pistol.activeInHierarchy)
            {
                int pistolLayerIndex = animator.GetLayerIndex("PistolLayer");
                if (pistolLayerIndex != -1) animator.SetLayerWeight(pistolLayerIndex, 1f);

                int axeLayerIndex = animator.GetLayerIndex("AxeLayer");
                if (axeLayerIndex != -1) animator.SetLayerWeight(axeLayerIndex, 0f);
            }
        }

        if (Input.GetMouseButton(0) && weapon != null && weapon.CurrentAmmo > 0 && !weapon.IsReloading && pistol.activeInHierarchy && !axe.activeInHierarchy)
        {
            animator.SetBool("isShooting", true);
        }
        else
        {
            animator.SetBool("isShooting", false);
        }

        if (Input.GetKeyDown(KeyCode.R) && weapon != null && weapon.IsReloading)
        {
            animator.SetBool("isReloading", true);
        }
        else
        {
            animator.SetBool("isReloading", false);
        }
    }
}
    