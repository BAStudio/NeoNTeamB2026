using UnityEngine;
using System.Collections;
using System.Diagnostics.Tracing;

public class ElevatorMoving : MonoBehaviour
{
    [Header("Elevator")]
    [SerializeField] private Transform elevator;
    [SerializeField] private Transform Player;
    [SerializeField] private GameObject brokenparticleeffects;
    [SerializeField] private GameObject AbovebunkerLightingAtmosphere;


    [Header("Settings")]
    [SerializeField] private float elevatorSpeed = 2f;
    [SerializeField] private float elevatorTravelDistance = 25f;
    [SerializeField] private bool elevatorIsUp = true;
    [SerializeField] private bool elevatorIsMoving = false;
    [SerializeField] private bool elevatorIsBroken = false;

    private Vector3 elevatorStartLoc;
    private Vector3 elevatorStopLoc;

    private void Update()
    {
        if (elevatorIsBroken == true)
        {
            AbovebunkerLightingAtmosphere.SetActive(false);
            gameObject.BroadcastMessage("StartEncounter");
            brokenparticleeffects.SetActive(true);
        }
    }
    void Start()
    {
        elevatorStopLoc = elevator.localPosition;
        elevatorStartLoc = elevatorStopLoc + new Vector3(0, elevatorTravelDistance, 0);
    }

    private IEnumerator MoveElevator(Vector3 targetPosition)
    {
        while (Vector3.Distance(elevator.localPosition, targetPosition) > 0.01f)
        {
            elevator.localPosition = Vector3.MoveTowards(
                elevator.localPosition,
                targetPosition,
                elevatorSpeed * Time.deltaTime
            );

            yield return null;
        }

        elevator.localPosition = targetPosition;
        elevatorIsBroken = true; elevatorIsUp = false;
        elevatorIsMoving = false;
        Player.SetParent(null);
        gameObject.BroadcastMessage("elovatorArrived");
        
    }

    public void StartElevator()
    {
        if (elevatorIsMoving || elevatorIsBroken) return;
        Player.SetParent(elevator);
        elevatorIsMoving = true;

        Vector3 target = elevatorIsUp ? elevatorStartLoc : elevatorStopLoc;
        StartCoroutine(MoveElevator(target));
    }
}