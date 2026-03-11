using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine.Audio;

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

    [Header("Microhpone")]
    [SerializeField] private float hearingrange = 10f;
    [SerializeField] private float hearingfieldofview = 360f;
    [SerializeField] private float hearingrotatespeed = 60f;
    [SerializeField] private float micSensitivity = 100f;
    [SerializeField] private float micThreshold = 0.05f;
    [SerializeField] private int sampleWindow = 128;

    private AudioClip micclip;
    private string micDevice;
    private bool heardplayer;
    private Vector3 lastheardposition;

    [SerializeField] private bool abletoseeplayer;
    [SerializeField] private bool canusemicrohpone;


    private void Start()
    {
        //animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        //animator.SetBool("IsWalking", true);
        if (Microphone.devices.Length > 0)
        {
            micDevice = Microphone.devices[0];
            micclip = Microphone.Start(micDevice, true, 10, 44100);
            Debug.Log("Microphone found: " + micDevice);
        }
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
        if (canusemicrohpone == true)
        {
            if (micclip != null)
            {
                int micPos = Microphone.GetPosition(micDevice) - sampleWindow;
                if (micPos < 0) return;

                float[] samples = new float[sampleWindow];
                micclip.GetData(samples, micPos);

                float level = 0f;

                for (int i = 0; i < sampleWindow; i++)
                {
                    level += Mathf.Abs(samples[i]);
                }

                level = (level / sampleWindow) * micSensitivity;

                if (level > micThreshold)
                {
                    heardplayer = true;
                    lastheardposition = Player.transform.position;
                }
                else
                {
                    heardplayer = false;
                }
            }
            //        heardplayer = true;
            //        lastheardposition = Player.transform.position;
            //        Debug.Log("Heard player at position: " + lastheardposition);
            //    }
            //    else
            //    {
            //        heardplayer = false;
            //    }
        }
        distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        {

            if (heardplayer) {
                Vector3 directionToLastHeard = (lastheardposition - transform.position).normalized;
                Quaternion targetRot = Quaternion.LookRotation(directionToLastHeard, Vector3.up);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    hearingrotatespeed * Time.deltaTime
                    );
                agent.SetDestination(lastheardposition);
            }

            RaycastHit hit;

            if (Player == null) return;

            Vector3 directionToPlayer = (Player.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
            if (Vector3.Angle(transform.forward, directionToPlayer) < fieldofview / 1.2f)
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
            if (indistance == true)
            {
            }
            else
            {
                timer += Time.deltaTime;

                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderingradius);
                    agent.SetDestination(newPos);
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
