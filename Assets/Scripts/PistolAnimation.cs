using UnityEngine;

public class PistolAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private WeaponBase weapon;
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
        if (Input.GetMouseButton(0) && weapon != null && weapon.CurrentAmmo > 0 && !weapon.IsReloading)
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
    