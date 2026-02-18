using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DoorLocked : MonoBehaviour
{
    [SerializeField] private  bool _isLocked = false;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _lockedSound;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject == gameObject)
                {
                    TryOpenDoor();
                }
            }

            
        }
    }

    private void TryOpenDoor()
    {
        if (_isLocked)
        {
           
            _audioSource.PlayOneShot(_lockedSound);
        }
        else
        {
            Debug.Log("the open");
           

        }
       
    }



}
