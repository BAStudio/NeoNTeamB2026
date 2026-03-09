using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private PlayerCam playerCam;

    private void LateUpdate()
    {
        transform.position = cameraPosition.position + playerCam.GetBobOffset();
    }
}