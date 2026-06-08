using UnityEngine;
using System;

public class TorsoDamage : DamageBase
{
    [SerializeField] private AudioSource Aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TorsoDamage = 0.0f;
        Mathf.Clamp(TorsoDamage, 0.0f, 3.0f);
    }   
    [System.Obsolete]
    protected virtual void Awake()
    {
        if (playerMovement == null) playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Rightlegisbroken && TorsoDamage >= 3f)
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
            Debug.Log("Right leg damaged" + TorsoDamage);
            TorsoDamage = Mathf.Clamp(TorsoDamage + 1f, 0f, 3f);
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
        TorsoisBroken = false;
        TorsoDamage = Mathf.Clamp(TorsoDamage - 1f, 0f, 3f);
        playerMovement.ResetSpeed();
    }
}