using UnityEngine;
using System;

public class TorsoDamage : DamageBase
{
    [SerializeField] private AudioSource Aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TorsoDamage = 0.0f;
        TorsoDamage = Mathf.Clamp(TorsoDamage, 0.0f, 3.0f);
    }
        
    [System.Obsolete]
    protected virtual void Awake() //deprecated?
    {
        if (playerMovement == null) playerMovement = FindObjectOfType<PlayerMovement>(); //deprecated, gebruik FindFirstObjectByType
    }

    // Update is called once per frame
    void Update()
    {
        if (!TorsoisBroken && TorsoDamage >= 3f)
        {
            damageDebuff();
            TorsoisBroken = true;
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with " + other.gameObject.tag);
        if (other.gameObject.tag == "damage")
        {
            Debug.Log("Torso damaged " + TorsoDamage);
            TorsoDamage = Mathf.Clamp(TorsoDamage + 1f, 0f, 3f);
            FullyBodyHealth -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("Torso damaged, movement speed reduced");
        playerMovement.ApplySpeedMultiplier(0.6f);
        Aud.Play();
    }

    public void TorsoHealed()
    {
        Debug.Log("Torso healed, movement speed restored");
        TorsoisBroken = false;
        TorsoDamage = Mathf.Clamp(TorsoDamage - 1f, 0f, 3f);
        playerMovement.ResetSpeed();
    }
}