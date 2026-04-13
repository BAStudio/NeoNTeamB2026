using UnityEngine;
using UnityEngine.UI;

public class ImageSpinning : MonoBehaviour
{
    [SerializeField] private int rotationSpeed = 100;

    [SerializeField] private Image spinImage;

    void Update()
    {
       
        spinImage.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        foreach (Transform child in spinImage.transform)
        {
            child.rotation = Quaternion.identity;
        }
    }
}