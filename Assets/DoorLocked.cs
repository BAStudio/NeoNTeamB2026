using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class DoorLocked : MonoBehaviour
{
    [SerializeField] private bool _isLocked = true;
    [SerializeField] private AudioClip _lockedSound;
    [SerializeField] private AudioSource _audioSource;

    public bool IsLocked => _isLocked; // Andere scripts mogen lezen of de deur locked is  Maar ze mogen het NIET aanpassen
    private void Update()
    {
     
        
    }
    public void Lock()
    {
        _isLocked = true;
        Debug.Log("Door is now LOCKED");
      
    }

    public void Unlock()
    {
        _isLocked = false;
        Debug.Log("Door is now UNLOCKED");
        
    }
    public void PlayLockedSound()
    {
        if (_isLocked)
        {
            _audioSource.PlayOneShot(_lockedSound);
        }
    }
       
    
    



}
