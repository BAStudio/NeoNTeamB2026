using UnityEngine;
using System;

public class RightLegDamage : DamageBase
{
    [SerializeField] private AudioSource Aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RightLegDamage = 0.0f;
        Health = 100.0f;
        Mathf.Clamp(RightLegDamage, 0.0f, 3.0f);

    }
    [System.Obsolete]
    protected virtual void Awake()
    {
        if (playerMovement == null) playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Rightlegisbroken && RightLegDamage >= 3f)
        {
            damageDebuff();
            Rightlegisbroken = true;
            return;
        }
        if (Rightlegisbroken && RightLegDamage < 3f)
        {
            Rightlegisbroken = false;
            healedLimb();
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with " + other.gameObject.tag);
        if (other.gameObject.tag == "damage")
        {
            Debug.Log("Right leg damaged" + RightLegDamage);
            RightLegDamage += 1;
            Health -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("Right leg damaged, movement speed reduced");
        playerMovement.ApplySpeedMultiplier(0.6f);
        Aud.Play();
    }

    private void healedLimb()
    {
        Debug.Log("Right leg healed, movement speed restored");
    }
}