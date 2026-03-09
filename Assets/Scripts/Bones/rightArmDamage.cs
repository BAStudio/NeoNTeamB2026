using System;
using UnityEngine;

public class RightArmDamage : DamageBase
{
    [SerializeField] private AudioSource Aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("rightArmDamage active");
        RightArmDamage = 0.0f;
        Health = 100.0f;

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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "damage")
        {
            Debug.Log("Right Arm damaged" + RightArmDamage);
            RightArmDamage += 1;
            Health -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("Right arm damaged, movement speed reduced");
        Aud.Play();
    }

    private void healing()
    {
        Debug.Log("Right arm healed, movement speed restored");
    }
}

