using UnityEngine;

public class LeftLegDamage : damageBase
{
    [SerializeField] private AudioSource Aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("LeftLegDamage active");
        LimbDamage = 0.0f;
        Health = 100.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (!Leftlegisbroken && LimbDamage >= 3f)
        {
            damageDebuff();
            Leftlegisbroken = true;
            return;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "damage")
        {
            Debug.Log("Left leg damaged" + LimbDamage);
            LimbDamage += 1;
            Health -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("Left leg damaged, movement speed reduced");
        Aud.Play();
    }

    private void healing() { }
}
