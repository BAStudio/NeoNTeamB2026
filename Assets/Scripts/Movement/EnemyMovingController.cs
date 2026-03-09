using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.AI.Navigation;

public class EnemyMovingController : MonoBehaviour
{
    public GameObject Player;
    float speed = 1f;
    private float timer;
    [SerializeField] private Animator animator;
    public NavMeshAgent agent;
    [SerializeField] private float distanceToPlayer;
    private bool indistance;
    [SerializeField] private float wanderingradius = 10f;
    [SerializeField] private float wanderingtimer = 5f;
    [SerializeField] private float fieldofview = 30f;
    [SerializeField] private float rotatespeed = 5f;

    [SerializeField] private bool abletoseeplayer;
    [SerializeField] private bool canusemicrohpone;


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
            agent.SetDestination(Player.transform.position);
            indistance = true;
        }
        else
        {
            RaycastHit hit;

            if (Player == null) return;

            Vector3 directionToPlayer = (Player.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
            if (Vector3.Angle(transform.forward, directionToPlayer) < fieldofview / 2.2f)
            {
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, distanceToPlayer /*, layerMask */))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        agent.SetDestination(Player.transform.position);
                        Quaternion targetRot = Quaternion.LookRotation(directionToPlayer, Vector3.up);
                        transform.rotation = Quaternion.Slerp(
                            transform.rotation,
                            targetRot,
                            rotatespeed * Time.deltaTime
                            );
                        indistance = true;

                    }
                    else
                    {
                        indistance = false;
                    }
                }
                else
                {
                    indistance = false;
                }
            }
            else
            {
                indistance = false;
            }



            if (indistance == true)
            {
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
