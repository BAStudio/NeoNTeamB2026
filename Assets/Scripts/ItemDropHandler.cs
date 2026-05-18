using UnityEngine;

public class ItemDropHandler : MonoBehaviour
{
    public float dropForce = 5f;

    public void Drop(GameObject item)
    {
        item.transform.SetParent(null);
        item.SetActive(true);

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(Camera.main.transform.forward * dropForce, ForceMode.Impulse);
        }
    }
}
