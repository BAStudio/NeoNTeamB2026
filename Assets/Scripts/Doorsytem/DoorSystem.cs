using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private bool isOpen = false;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject turningPoint;
    [SerializeField] private float openSpeed = 2f;


    [Header("Input")]
    [SerializeField] KeyCode OpenDoorKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode lockKey = KeyCode.F;
    [SerializeField] private KeyCode unlockKey = KeyCode.E;


    [Header("Audio")]
    [SerializeField] private AudioClip doorSound;
    [SerializeField] private AudioSource audioSource;


     private DoorLocked doorLocked;

   

    void Start()
    {
        doorLocked = GetComponent<DoorLocked>();

    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))

        {
         
            if (hit.collider.gameObject == door)
            {

                if (Input.GetKeyDown(lockKey))
                {
                    if (doorLocked != null) doorLocked.Lock();

                    isOpen = false;
                }


                if (Input.GetKeyDown(unlockKey))
                {
                    if (doorLocked != null) doorLocked.Unlock();
                }



                if (Input.GetMouseButtonDown(0))
                {
                    if (doorLocked != null && doorLocked.IsLocked)
                    {
                        doorLocked.PlayLockedSound();
                        return;
                    }

                    isOpen = !isOpen;


                    audioSource.PlayOneShot(doorSound);
                }
            }
        }

        SmoothRotate();
    }




    private void SmoothRotate() 
    {  Quaternion targetRotation = isOpen ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, 0, 0);
       turningPoint.transform.rotation = Quaternion.Slerp(turningPoint.transform.rotation, targetRotation, Time.deltaTime * openSpeed);
    }


}
