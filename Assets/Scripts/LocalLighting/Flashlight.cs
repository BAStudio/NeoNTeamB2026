using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Light flashlightLight;
    [SerializeField] private bool isOn = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
    }
    void ToggleFlashlight()
    {
        isOn = !isOn;
        flashlightLight.enabled = isOn;
    }
}
