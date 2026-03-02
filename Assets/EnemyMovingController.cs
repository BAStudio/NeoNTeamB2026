using UnityEngine;
using UnityEngine.AI;

public class EnemyMovingController : MonoBehaviour
{
    public GameObject Player;
    float speed = 3f;
    [SerializeField] private Animator animator;
    public NavMeshAgent agent;
    [SerializeField] private float distanceToPlayer;
    private bool indistance;



    private void Start()
    {
        //animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        //animator.SetBool("IsWalking", true);
    }
    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (distanceToPlayer < 5f)
        {
            indistance = true;
        }
        else
        {
            indistance = false;
        }

        if (indistance == true)
        {
            agent.SetDestination(Player.transform.position);
            //animator.SetBool("IsWalking", true);
            if (distanceToPlayer < 2f)
            {
                //animator.SetBool("attac", true);
            }
            if (distanceToPlayer > 5f)
            {
                indistance = false;
                //animator.SetBool("IsWalking", false);
            }
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }

    private void MoveTowardsPlayer()
    {


    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.gameObject == Player)
        {
            animator.SetBool("attac", true);
            Debug.Log($"doe het pls");
        }
    }

}
