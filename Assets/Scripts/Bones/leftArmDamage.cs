using UnityEngine;

public class LeftArmDamage : DamageBase
{
    [SerializeField] private AudioSource Aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("leftArmDamage active");
        LeftArmDamage = 0.0f;
        Health = 100.0f;

    }
    protected virtual void Awake()
    {
        if (playerMovement == null) playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Leftarmisbroken && LeftArmDamage >= 3f)
        {
            damageDebuff();
            Leftarmisbroken = true;
            return;
        }
        if (Leftarmisbroken && LeftArmDamage < 3f)
        {
            Leftarmisbroken = false;
            healing();
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "damage")
        {
            Debug.Log("Left arm damaged" + RightArmDamage);
            LeftArmDamage += 1;
            Health -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("Left arm damaged, movement speed reduced");
        //playerMovement.ApplySpeedMultiplier(0.75f);
        Aud.Play();
    }

    private void healing()
    {
        Debug.Log("Left arm healed, movement speed restored");
    }
}
