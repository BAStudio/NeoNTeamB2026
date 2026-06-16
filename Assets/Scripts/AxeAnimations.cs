using UnityEngine;

public class AxeAnimations : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject axe;
    [SerializeField] private GameObject pistol;

    private bool playLeft = true;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float attackDuration = 1.05f; // 1 second animation length

    private void Update()
    {
        if (animator != null)
        {
            if (axe != null && axe.activeInHierarchy)
            {
                int axeLayerIndex = animator.GetLayerIndex("AxeLayer");
                if (axeLayerIndex != -1) animator.SetLayerWeight(axeLayerIndex, 1f);

                int pistolLayerIndex = animator.GetLayerIndex("PistolLayer");
                if (pistolLayerIndex != -1) animator.SetLayerWeight(pistolLayerIndex, 0f);
            }
        }

        // Handle attack timer
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                isAttacking = false;
                animator.SetBool("HitLeft", false);
                animator.SetBool("HitRight", false);
            }
        }

        // Only allow a new attack if we are not currently attacking
        if (Input.GetMouseButtonDown(0) && axe.activeInHierarchy && !pistol.activeInHierarchy && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackDuration;

            if (playLeft)
            {
                animator.SetBool("HitLeft", true);
                animator.SetBool("HitRight", false);
            }
            else
            {
                animator.SetBool("HitRight", true);
                animator.SetBool("HitLeft", false);
            }

            playLeft = !playLeft;
        }
    }
}
