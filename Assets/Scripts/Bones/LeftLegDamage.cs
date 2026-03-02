using UnityEngine;

public class LeftLegDamage : damageBase
{
    [SerializeField] private AudioSource Aud;
    private bool isDebuffed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("legDamage active");
        LimbDamage = 0.0f;
        Health = 100.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDebuffed && LimbDamage >= 3f)
        {
            damageDebuff();
            isDebuffed = true;
            return;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "damage")
        {
            Debug.Log("leg damaged" + LimbDamage);
            LimbDamage += 1;
            Health -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("leg damaged, movement speed reduced");
        Aud.Play();
    }

    private void healing() { }
}
