using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Pistol pistolScript;

    void Start()
    {
        if (pistolScript == null)
        {
            pistolScript = FindObjectOfType<Pistol>();
        }
    }

    void Update()
    {
        if (pistolScript == null)
        {
            pistolScript = FindObjectOfType<Pistol>();
        }

        if (pistolScript != null)
        {
            ammoText.text = pistolScript.CurrentAmmo.ToString() + " / " + pistolScript.MaxAmmo.ToString();
        }
    }
}
