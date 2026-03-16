using System;
using UnityEngine;

public class RightArmDamage : DamageBase
{
    [SerializeField] private AudioSource Aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        RightArmDamage = 0.0f;
        Health = 100.0f;

    }

    protected virtual void Awake()
    {
        if (playerMovement == null) playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Rightarmisbroken && RightArmDamage >= 3f)
        {
            damageDebuff();
            Rightarmisbroken = true;
            return;
        }
        if (Rightarmisbroken && RightArmDamage < 3f)
        {
            Rightarmisbroken = false;
            healing();
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "damage")
        {
            Debug.Log("Right Arm damaged" + RightArmDamage);
            RightArmDamage += 1;
            Health -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("Right arm broken, can't use heavy weapons");
        //playerMovement.ApplySpeedMultiplier(0.75f); // debuff
        Aud.Play();
    }

    private void healing()
    {
        Debug.Log("Right arm healed, heavy weapon usage restored");
        playerMovement.ResetSpeed(); // healing
    }


}

