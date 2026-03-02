using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.AI.Navigation;

public class EnemyMovingController : MonoBehaviour
{
    public GameObject Player;
    float speed = 1f;
    [SerializeField] private Animator animator;
    public NavMeshAgent agent;
    [SerializeField] private float distanceToPlayer;
    private bool indistance;
    [SerializeField] private float wanderingradius = 10f;
    [SerializeField] private float wanderingtimer = 5f;
    private float timer;


    private void Start()
    {
        //animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        //animator.SetBool("IsWalking", true);
    }
    private Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, NavMesh.AllAreas);

        return navHit.position;
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
            timer += Time.deltaTime;

            if (timer >= wanderingtimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderingradius);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }

    }

    private void OnAnimatorMove()
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
