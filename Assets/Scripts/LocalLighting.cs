using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
public class LocalLighting : MonoBehaviour
{
    [Header("Lightingsettings")]
    [SerializeField] private Light currentlight;
    [SerializeField] private Rigidbody LightFallBody;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject brokenlight;
    [SerializeField] private AudioSource breakslightsource;

    [Header("Flicker Settings")]
    [SerializeField] private float flickerIntensity = 0.5f;
    [SerializeField] private float flickerSpeed = 4;
    [SerializeField] private float EnableLightAfterDelay = 5f;

    [Header("Main Settings")]
    [SerializeField] bool flickerEnabled;
    [SerializeField] bool canbreak;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (flickerEnabled)
        {
            if (player != null && currentlight != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToPlayer < 5f)
                {
                    FlickerLight();
                }
                else
                {
                }
            }
        }
        if (canbreak)
        {

            if (player != null && currentlight != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToPlayer < 5f)
                {
                    currentlight.enabled = false;
                    GetComponent<Rigidbody>().useGravity = true;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            breakslightsource.PlayDelayed(0.2f);
            Instantiate(brokenlight, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    private void FlickerLight()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        currentlight.intensity = Mathf.Lerp(currentlight.intensity, flickerIntensity, noise);
    }
}
