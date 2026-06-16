using UnityEngine;
using System.Collections;

public class SoundEmitter : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;   // Drag your AudioSource here in the Inspector
    public AudioClip clip;           // Optional: override clip here

    private void Start()
    {
        if (gameObject.tag == "SoundEmitter" && audioSource != null)
        {
            if (clip != null)
            {
                audioSource.clip = clip;
            }
            audioSource.Play();
        }
    }
}