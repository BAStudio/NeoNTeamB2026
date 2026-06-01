using UnityEngine;
using UnityEngine.UI;

public class UIMask : MonoBehaviour
{
    [SerializeField] private Material spotlightMaterial;
    [SerializeField] RectTransform canvasRect; 

    // Update is called once per frame
    void Update()
    {
        Vector2 localPoint; 

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, null, out localPoint);

        float normalizedX = (localPoint.x + canvasRect.rect.width / 2) / canvasRect.rect.width;
        float normalizedY = (localPoint.y + canvasRect.rect.height / 2) / canvasRect.rect.height;

        spotlightMaterial.SetVector("_MousePos", new Vector4(normalizedX, normalizedY, 0, 0));

    }
}
