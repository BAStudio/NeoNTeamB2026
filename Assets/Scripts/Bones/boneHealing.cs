using UnityEngine;

public class boneHealing : damageBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LimbDamage >= 3)
        { 
          
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "healing")
        {
            Debug.Log("Bone healed" + LimbDamage);
            LimbDamage -= 1;
            Health += 10;
            GameObject.Destroy(gameObject);
        }
    }
}
