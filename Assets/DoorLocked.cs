using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class DoorLocked : MonoBehaviour
{
    [Header("Locking")]
    [SerializeField] private bool isLocked = true;

    [Header("Audio")]
    [SerializeField] private AudioClip lockedSound;
    [SerializeField] private AudioSource audioSource;

    public bool IsLocked => isLocked; 
    private void Update()
    {
     
        
    }
    public void Lock()
    {
        isLocked = true;
        Debug.Log("Door is now LOCKED");
      
    }

    public void Unlock()
    {
        isLocked = false;
        Debug.Log("Door is now UNLOCKED");
        
    }
    public void PlayLockedSound()
    {
        if (isLocked)
        {
            audioSource.PlayOneShot(lockedSound);
        }
    }
       
    
    



}
