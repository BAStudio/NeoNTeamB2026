using UnityEngine;

public class AxeLock : MonoBehaviour
{
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float attackDelay = 0.4f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private Camera cam;
    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;

    private void Attack()
    {
        if (!readyToAttack || attacking) return;
        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance))
        {
            Debug.Log("Hit: " + hit.collider.name);
            Lock lockObject = hit.collider.GetComponent<Lock>();
            if (lockObject != null)
            {
                lockObject.TakeHit();
            }
        }
    }

}
