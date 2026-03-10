using UnityEngine;

public class LeftLegDamage : DamageBase
{
    [SerializeField] private AudioSource Aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("LeftLegDamage active");
        LeftLegDamage = 0f;
        Health = 100.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (!Leftlegisbroken && LeftLegDamage >= 3f)
        {
            damageDebuff();
            Leftlegisbroken = true;
            return;
        }
        if (Leftlegisbroken && LeftLegDamage < 3f)
        {
            Leftlegisbroken = false;
            healing();
            return;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "damage")
        {
            Debug.Log("Left leg damaged" + RightArmDamage);
            LeftLegDamage += 1;
            Health -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("Left leg damaged, movement speed reduced");
        Aud.Play();
    }

    private void healing()
    {
        Debug.Log("Left leg healed, movement speed restored");
    }
}

