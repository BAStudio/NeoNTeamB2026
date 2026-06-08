using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Pistol pistolScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = pistolScript.CurrentAmmo.ToString() + " / " + pistolScript.MaxAmmo.ToString();
    }
}
