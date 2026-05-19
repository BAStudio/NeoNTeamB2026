using UnityEngine;

public class LeftLegDamage : DamageBase
{
    [SerializeField] private AudioSource Aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeftLegDamage = 0f;
        Mathf.Clamp(LeftLegDamage, 0.0f, 3.0f);
    }

    [System.Obsolete]
    protected virtual void Awake()
    {
        if (playerMovement == null) playerMovement = FindObjectOfType<PlayerMovement>();
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

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with " + other.gameObject.tag);
        if (other.gameObject.tag == "damage")
        {
            Debug.Log("Left leg damaged" + RightArmDamage);
            LeftLegDamage = Mathf.Clamp(LeftLegDamage + 1f, 0f, 3f);
            FullyBodyHealth -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("Left leg damaged, movement speed reduced");
        playerMovement.ApplySpeedMultiplier(0.6f);
        Aud.Play();
    }

    public void HealLeftLeg()
    {
        Debug.Log("Left leg healed, movement speed restored");
        Leftlegisbroken = false;
        LeftLegDamage = Mathf.Clamp(LeftLegDamage - 1f, 0f, 3f);
        playerMovement.ResetSpeed();
    }
}

