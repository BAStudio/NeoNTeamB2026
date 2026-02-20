using UnityEngine;

public class legDamage : damageBase
{
    [SerializeField] private AudioSource Aud;
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
        if (LimbDamage == 3f)
        {
            damageDebuff();
        }
        }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "damage")
        {
            Debug.Log("leg damaged" + LimbDamage);
            LimbDamage+= 1;
            Health -= 10;
        }
    }
    private void damageDebuff()
    {
        Debug.Log("leg damaged, movement speed reduced");
        Aud.Play();
    }
}
