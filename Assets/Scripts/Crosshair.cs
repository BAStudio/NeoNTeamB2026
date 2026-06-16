using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private RectTransform Top, Bottom, Left, Right;
    [SerializeField] private WeaponBase weapon;
    [SerializeField] private Canvas canvas;


    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (weapon == null) return;
        float spread = 1f - weapon.Accuracy;
        Vector3 forwardPoint = weapon.transform.position + weapon.transform.forward;
        Vector3 screenCenter = cam.WorldToScreenPoint(forwardPoint);
        Vector3 screenUp = cam.WorldToScreenPoint(forwardPoint + cam.transform.up * spread);
        float pixelSpread = (screenUp.y - screenCenter.y) / canvas.scaleFactor;
        Top.anchoredPosition = new Vector2(0f, pixelSpread);
        Bottom.anchoredPosition = new Vector2(0f, -pixelSpread);
        Left.anchoredPosition = new Vector2(-pixelSpread, 0f);
        Right.anchoredPosition = new Vector2(pixelSpread, 0f);
    }
}
