using UnityEngine;

public class legDamage : damageBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LimbDamage = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "damage")
        {
            LimbDamage--;
        }
    }
}
