using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    [SerializeField] private bool isOpen = false;
    [SerializeField] private GameObject Door;
    [SerializeField] private GameObject TurningPoint;
    public KeyCode OpenDoorKey = KeyCode.Mouse0;
    [SerializeField] private float OpenSpeed = 2f;
    [SerializeField] private AudioClip DoorSound;
    [SerializeField] private AudioSource _audioSource;

     private DoorLocked doorLocked;

    [SerializeField] private KeyCode LockKey = KeyCode.F;
    [SerializeField] private KeyCode UnlockKey = KeyCode.E;

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
         
            if (hit.collider.gameObject == Door)
            {

                if (Input.GetKeyDown(LockKey))
                {
                    if (doorLocked != null) doorLocked.Lock();

                    // deur meteen dicht als je lockt
                    isOpen = false;
                }

                // UNLOCK
                if (Input.GetKeyDown(UnlockKey))
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


                    _audioSource.PlayOneShot(DoorSound);
                }
            }
        }

        SmoothRotate();
    }




    private void SmoothRotate() 
    {         Quaternion targetRotation = isOpen ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, 0, 0);
        TurningPoint.transform.rotation = Quaternion.Slerp(TurningPoint.transform.rotation, targetRotation, Time.deltaTime * OpenSpeed);
    }


}
