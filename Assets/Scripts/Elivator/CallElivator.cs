using UnityEngine;
using System.Collections;

public class CallElivator : MonoBehaviour
{
    [SerializeField] private KeyCode callKey = KeyCode.E;
    [SerializeField] private float callDistance = 3f;
    [SerializeField] private Transform player;
    [Header("Elivator")]
    [SerializeField] private GameObject ElivButton;
    [SerializeField] private GameObject ElivButtonInside;
    [SerializeField] private Transform Leftdoor;
    [SerializeField] private Transform Rightdoor;

    [Header("Audio")]
    [SerializeField] private AudioSource ElivatorMusic;
    [SerializeField] private AudioSource Elivatorarivalding;

    [Header("Settings")]
    [SerializeField] private float ElivatorMusicVolume = 0.5f;
    [SerializeField] private float ElivatorarivaldingVolume = 0.5f;
    [SerializeField] private float DoorSpeed = 2f;

    private bool elevatorCalled = false;
    private bool doorsclosed = false;
    private bool doorslocked = false;

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightOpenPos;

    private void Start()
    {
        leftClosedPos = Leftdoor.localPosition;
        rightClosedPos = Rightdoor.localPosition;

        leftOpenPos = leftClosedPos + new Vector3(0, 0, -0.85f);
        rightOpenPos = rightClosedPos + new Vector3(0, 0, 0.85f);
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < 5f)
        {
            if (doorslocked == false)
            {
                if (Input.GetKeyDown(KeyCode.E) && !elevatorCalled)
                {
                    StartCoroutine(CallElevator());
                    doorsclosed = false;
                }
            }
        }
        if (distanceToPlayer < 2f && doorsclosed == true)
        {
            doorslocked = true;
            gameObject.BroadcastMessage("StartElevator");
        }
    }

    private IEnumerator CallElevator()
    {
        elevatorCalled = true;

        ElivatorMusic.volume = ElivatorMusicVolume;
        Elivatorarivalding.volume = ElivatorarivaldingVolume;

        ElivatorMusic.Play();
        yield return new WaitForSeconds(2f);

        Elivatorarivalding.Play();

        yield return StartCoroutine(MoveDoors(leftOpenPos, rightOpenPos));
        doorsclosed = false;
        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(MoveDoors(leftClosedPos, rightClosedPos));
        doorsclosed = true;
        elevatorCalled = false;
    }
    public void elovatorArrived()
    {
        StartCoroutine(MoveDoors(leftOpenPos, rightOpenPos));
    }
    private IEnumerator MoveDoors(Vector3 leftTarget, Vector3 rightTarget)
    {
        while (Vector3.Distance(Leftdoor.localPosition, leftTarget) > 0.01f)
        {
            Leftdoor.localPosition = Vector3.Lerp(Leftdoor.localPosition, leftTarget, Time.deltaTime * DoorSpeed);
            Rightdoor.localPosition = Vector3.Lerp(Rightdoor.localPosition, rightTarget, Time.deltaTime * DoorSpeed);
            yield return null;
        }
    }
}