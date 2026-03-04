using System;
using UnityEngine;

public class rightArmDamage : damageBase
{
    [SerializeField] private AudioSource Aud;
    public static event Action HealedArmRight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("rightArmDamage active");
        LimbDamage = 0.0f;
        Health = 100.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (!Rightarmisbroken && LimbDamage >= 3f)
        {
            damageDebuff();
            Rightarmisbroken = true;
            return;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "damage")
        {
            Debug.Log("Right Arm damaged" + LimbDamage);
            LimbDamage += 1;
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
        HealedArmRight?.Invoke();
    }
}
