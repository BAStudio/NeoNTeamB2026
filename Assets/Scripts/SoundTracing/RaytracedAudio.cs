using UnityEngine;

public class RaytracedAudio : MonoBehaviour
{
    public Transform audioSource;
    public Transform listener;
    public LayerMask occlusionLayers;
    public float maxDistance = 100f;
    public float occlusionFactor = 0.5f;

    private AudioSource audioSourceComponent;

    void Start()
    {
        audioSourceComponent = audioSource.GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 direction = listener.position - audioSource.position;
        float distance = direction.magnitude;

        if (distance > maxDistance)
        {
            audioSourceComponent.volume = 0f;
            return;
        }

        Ray ray = new Ray(audioSource.position, direction.normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, distance, occlusionLayers))
        {
            // If the ray hits an obstacle, reduce the volume based on the occlusion factor
            audioSourceComponent.volume = Mathf.Lerp(1f, occlusionFactor, hit.distance / distance);
        }
        else
        {
            // If there are no obstacles, set the volume to full
            audioSourceComponent.volume = 1f;
        }

    }
}
