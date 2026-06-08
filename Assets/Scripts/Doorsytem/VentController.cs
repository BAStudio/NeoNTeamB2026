using UnityEngine;

public class VentController : MonoBehaviour
{
    [Header("Screw Setup")]
    public VentScrew[] screws;   // Sleep hier je schroeven in
    public VentOpen vent;        // Sleep hier je vent in

    private int screwsPressed = 0;
    private bool isOpen = false;

    public void ScrewActivated(VentScrew screw)
    {
        if (isOpen) return;

        screw.SetActivated();

        screwsPressed++;

        Debug.Log("Screws pressed: " + screwsPressed + " / " + screws.Length);

        if (screwsPressed >= screws.Length)
        {
            OpenVent();
        }
    }

    void OpenVent()
    {
        isOpen = true;

        Debug.Log("All screws done. Opening vent!");

        if (vent != null)
        {
            vent.Open();
        }
    }
}