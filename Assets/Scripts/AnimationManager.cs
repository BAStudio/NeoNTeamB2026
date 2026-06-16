using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Animator animator2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("Shoot");
            animator2.SetTrigger("Shoot");
        }
    }
}
