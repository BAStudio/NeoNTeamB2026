using UnityEngine;

public class Aiming : MonoBehaviour
{
    [SerializeField] Vector3 aimCam;
    [SerializeField] float aimSpeed = 8f;
    private Vector3 originalPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPos = transform.localPosition;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimCam, aimSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, aimSpeed * Time.deltaTime);
        }
    }
}
