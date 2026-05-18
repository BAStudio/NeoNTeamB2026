using UnityEngine;

public class HandItemHandler : MonoBehaviour
{
    public Transform handTransform;

    public void ShowItem(GameObject item)
    {
        item.SetActive(true);
        item.transform.SetParent(handTransform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }

    public void HideItem(GameObject item)
    {
        item.SetActive(false);
    }
}
