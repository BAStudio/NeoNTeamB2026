using UnityEngine;

public class LeftArmDamage : DamageBase
{
    [SerializeField] private AudioSource Aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeftArmDamage = 0.0f;
        Health = 100.0f;
        Mathf.Clamp(LeftArmDamage, 0.0f, 3.0f);
    }
    [System.Obsolete]
    protected virtual void Awake()
    {
        if (playerMovement == null) playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Leftarmisbroken && LeftArmDamage >= 3f)
        {
            Aud.Play();
            damageDebuff();
            Leftarmisbroken = true;
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "damage")
        {
            Debug.Log("Left arm damaged" + RightArmDamage);
            LeftArmDamage = Mathf.Clamp(LeftArmDamage + 1f, 0f, 3f);
            Health -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("Left arm broken, can't use heavy weapons.");
        //playerMovement.ApplySpeedMultiplier(0.75f);
        Aud.Play();
    }

    public void LeftArmHealing()
    {
        Leftarmisbroken = false;
        LeftArmDamage =- 1f;
        Debug.Log("Left arm healed, Use of heavy weapons possible");
    }
}
