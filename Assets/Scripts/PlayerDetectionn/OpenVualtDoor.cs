using UnityEngine;
using System.Collections;

public class OpenVualtDoor : MonoBehaviour
{
    [Header("Door gameobjects")]
    [SerializeField] private GameObject VualtDoor;
    [SerializeField] private GameObject ParticalObjecs;
    [SerializeField] private Transform player;

    [Header("Door open settings")]
    [SerializeField] private float openDoorSpeed = 2f;
    [SerializeField] private float playerdetectionArea = 5f;
    [SerializeField] private Vector3 openOffset;

    private bool isDoorOpen = false;
    private bool isMoving = false;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        closedPosition = VualtDoor.transform.position;
        openPosition = closedPosition + openOffset;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < playerdetectionArea && !isDoorOpen && !isMoving)
        {
            StartCoroutine(MoveDoor(openPosition));
            isDoorOpen = true;
        }
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        isMoving = true;
        ParticalObjecs.SetActive(true);

        while (Vector3.Distance(VualtDoor.transform.position, targetPosition) > 0.01f)
        {
            VualtDoor.transform.position = Vector3.MoveTowards(
                VualtDoor.transform.position,
                targetPosition,
                openDoorSpeed * Time.deltaTime
            );

            yield return null;
        }

        ParticalObjecs.SetActive(false);
        isMoving = false;
    }
}