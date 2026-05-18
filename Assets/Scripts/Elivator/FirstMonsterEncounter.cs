using UnityEngine;
using System.Collections;

// Simplified encounter: spawn the enemy, play an animation/audio, then remove the enemy.
public class FirstMonsterEncounter : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject BigZombie;
    [SerializeField] private GameObject Player;

    [Header("Positions")]
    [SerializeField] private Transform BigZombieSpawnPoint;

    [Header("Audio")]
    [SerializeField] private AudioSource BigZombieAudio;

    [Header("Settings")]
    [Tooltip("Animator trigger name to play on the spawned enemy. Leave empty to skip.")]
    [SerializeField] private string AnimationTrigger = "Spawn";
    [Tooltip("How long to wait for the animation before removing the enemy.")]
    [SerializeField] private float AnimationDuration = 2f;
    [Tooltip("Collider used to trigger the encounter. Assign the trigger GameObject's Collider (must be 'Is Trigger'). If left empty, the Collider on this GameObject will be used.")]
    [SerializeField] private Collider TriggerZone;

    private GameObject spawnedBigZombie;
    private bool encounterStarted = false;

    private void Start()
    {
        if (BigZombie != null && BigZombieSpawnPoint != null)
        {
            spawnedBigZombie = Instantiate(BigZombie, BigZombieSpawnPoint.position, BigZombieSpawnPoint.rotation);
            if (spawnedBigZombie != null)
                spawnedBigZombie.SetActive(false);
        }

        // If no trigger zone assigned, try to use the collider on this GameObject
        if (TriggerZone == null)
            TriggerZone = GetComponent<Collider>();

        if (TriggerZone == null)
            Debug.LogWarning("FirstMonsterEncounter: No TriggerZone assigned and no Collider found on the GameObject. Assign a trigger Collider to start the encounter.");
        else if (!TriggerZone.isTrigger)
            Debug.LogWarning("FirstMonsterEncounter: Assigned TriggerZone is not set as 'Is Trigger'. OnTriggerEnter requires an isTrigger collider.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (encounterStarted) return;
        if (TriggerZone == null) return; // no trigger zone configured

        // Only respond when the collider that entered belongs to the configured TriggerZone
        // Note: OnTriggerEnter will only be called on this MonoBehaviour if this GameObject has the trigger collider.
        // We therefore only need to check the 'other' to see if it's the player.
        if (Player != null)
        {
            // allow player collider or any child collider
            if (other.gameObject == Player || other.transform.IsChildOf(Player.transform))
            {
                StartEncounter();
            }
        }
        else
        {
            if (other.CompareTag("Player"))
                StartEncounter();
        }
    }

    private void StartEncounter()
    {
        if (encounterStarted) return;
        encounterStarted = true;

        if (spawnedBigZombie != null)
            spawnedBigZombie.SetActive(true);
        else if (BigZombie != null && BigZombieSpawnPoint != null)
            spawnedBigZombie = Instantiate(BigZombie, BigZombieSpawnPoint.position, BigZombieSpawnPoint.rotation);

        // Play audio
        if (BigZombieAudio != null)
            BigZombieAudio.Play();
        else if (spawnedBigZombie != null)
        {
            var a = spawnedBigZombie.GetComponentInChildren<AudioSource>();
            if (a != null) a.Play();
        }

        // Trigger animation if Animator exists
        if (!string.IsNullOrEmpty(AnimationTrigger) && spawnedBigZombie != null)
        {
            var animator = spawnedBigZombie.GetComponentInChildren<Animator>();
            if (animator != null)
                animator.SetTrigger(AnimationTrigger);
        }

        StartCoroutine(PlayAndRemoveRoutine(AnimationDuration));
    }

    private IEnumerator PlayAndRemoveRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (spawnedBigZombie != null)
            Destroy(spawnedBigZombie);
    }
}
