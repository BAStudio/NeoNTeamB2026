using UnityEngine;

public class rightLegDamage : damageBase
{
    [SerializeField] private AudioSource Aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("rightLegDamage");
        LimbDamage = 0.0f;
        Health = 100.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (!Rightlegisbroken && LimbDamage >= 3f)
        {
            damageDebuff();
            Rightlegisbroken = true;
            return;
        }
        if (Rightlegisbroken && LimbDamage < 3f)
        {
            Rightlegisbroken = false;
            healedLimb();
            return;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "damage")
        {
            Debug.Log("Right leg damaged" + LimbDamage);
            LimbDamage+= 1;
            Health -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("Right leg damaged, movement speed reduced");
        Aud.Play();
    }

    private void healedLimb()
    {
        Debug.Log("Right leg healed, movement speed restored");
    }
}