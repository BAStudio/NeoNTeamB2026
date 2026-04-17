using UnityEngine;
using System.Collections;

public class SoundEmitter : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;   // Drag your AudioSource here in the Inspector
    public AudioClip clip;           // Optional: override clip here

    [Header("Timing")]
    public float intervalSeconds = 3f; // How many seconds between plays

    private Coroutine playRoutine;

    private void Start()
    {
        // If no AudioSource assigned, try to get one on this GameObject
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Optionally, set the clip if provided
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
        }

        // Start the loop
        playRoutine = StartCoroutine(PlaySoundLoop());
    }

    private IEnumerator PlaySoundLoop()
    {
        while (true)
        {
            // Play the sound once; PlayOneShot is good for short effects[web:16][web:18]
            if (audioSource != null)
            {
                if (clip != null)
                    audioSource.PlayOneShot(clip);
                else
                    audioSource.Play(); // uses the clip on the AudioSource[web:16][web:18]
            }

            // Wait for the interval before playing again[web:19][web:21]
            yield return new WaitForSeconds(intervalSeconds);
        }
    }

    // Optional: stop from other scripts if needed
    public void StopLoop()
    {
        if (playRoutine != null)
        {
            StopCoroutine(playRoutine);
            playRoutine = null;
        }
    }
}