using System;
using UnityEngine;

public class RightArmDamage : DamageBase
{
    [SerializeField] private AudioSource Aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        RightArmDamage = 0.0f;
        Mathf.Clamp(RightArmDamage, 0.0f, 3.0f);
    }

    [System.Obsolete]
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
            Aud.Play();
            Rightarmisbroken = true;
            return;
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "damage")
        {
            Debug.Log("Right Arm damaged" + RightArmDamage);
            RightArmDamage = Mathf.Clamp(RightArmDamage + 1f, 0f, 3f);
            FullyBodyHealth -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("Right arm broken, can't use heavy weapons");
        Aud.Play();
    }

    public void RightArmHealing()
    {
        Debug.Log("Right arm healed, heavy weapon usage restored");
        Rightarmisbroken = false;
        RightArmDamage = Mathf.Clamp(RightArmDamage - 1f, 0f, 3f);
    }


}

