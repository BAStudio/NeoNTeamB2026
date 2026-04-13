using UnityEngine;
using TMPro;

public class InteractLift : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Camera camera;
    [SerializeField] Transform text;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(camera.transform.position, camera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("ElevatorButton"))
            {
                text.gameObject.SetActive(true);


            }
            else
            {
                text.gameObject.SetActive(false);
            }
        }

    }
}
