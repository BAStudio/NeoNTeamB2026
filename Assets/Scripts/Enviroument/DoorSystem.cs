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

    [SerializeField] private DoorLocked doorLocked;

    void Start()
    {


    }
    void Update()
    {
       
        if (Input.GetKeyDown(OpenDoorKey))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == Door)
                {
                    if (Input.GetKey(KeyCode.F))
                    {
                        Debug.Log("locked");
                        doorLocked.KeyLock();
                    }

                    _audioSource.PlayOneShot(DoorSound);
                    isOpen = !isOpen;
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
