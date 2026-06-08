using UnityEngine;

public class VentScrew : MonoBehaviour
{
    private bool isActivated = false;

    public void SetActivated()
    {
        if (isActivated) return;

        isActivated = true;
        Debug.Log(gameObject.name + " activated!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActivated) return;

        if (other.GetComponentInParent<Screwdriver>() != null)
        {
            FindObjectOfType<VentController>().ScrewActivated(this);
        }
    }
}