using UnityEngine;
using System;

public class HeadDamage : DamageBase
{
    [SerializeField] private AudioSource Aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HeadDamage = 0.0f;
        Mathf.Clamp(HeadDamage, 0.0f, 3.0f);
    }
    [System.Obsolete]
    protected virtual void Awake()
    {
        if (playerMovement == null) playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Headisbroken && HeadDamage >= 3f)
        {
            damageDebuff();
            Headisbroken = true;
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with " + other.gameObject.tag);
        if (other.gameObject.tag == "damage")
        {
            Debug.Log("Head damaged " + HeadDamage);
            HeadDamage = Mathf.Clamp(HeadDamage + 1f, 0f, 3f);
            FullyBodyHealth -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("Head damaged, movement speed reduced");
        playerMovement.ApplySpeedMultiplier(0.6f);
        Aud.Play();
    }

    public void HeadHealed()
    {
        Debug.Log("Head healed, movement speed restored");
        Headisbroken = false;
        HeadDamage = Mathf.Clamp(HeadDamage - 1f, 0f, 3f);
        playerMovement.ResetSpeed();
    }
}