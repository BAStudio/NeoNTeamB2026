using UnityEngine;
using System;

public class RightLegDamage : DamageBase
{
    [SerializeField] private AudioSource Aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RightLegDamage = 0.0f;
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
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with " + other.gameObject.tag);
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Right leg damaged" + RightLegDamage);
            RightLegDamage = Mathf.Clamp(RightLegDamage + 1f, 0f, 3f);
            FullyBodyHealth -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("Right leg damaged, movement speed reduced");
        playerMovement.ApplySpeedMultiplier(0.6f);
        Aud.Play();
    }

    public void RightLegHealed()
    {
        Debug.Log("Right leg healed, movement speed restored");
        Rightlegisbroken = false;
        RightLegDamage = Mathf.Clamp(RightLegDamage - 1f, 0f, 3f);
        playerMovement.ResetSpeed();
    }
}