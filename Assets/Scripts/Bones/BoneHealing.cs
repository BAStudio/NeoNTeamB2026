using UnityEngine;

public class BoneHealing : DamageBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Leftlegisbroken && collision.gameObject.tag =="healing")
        {
            Leftlegisbroken = false;
            LeftLegDamage -= 1;
            Debug.Log("Left leg healed" + LeftLegDamage);
        }
        if (Rightlegisbroken && collision.gameObject.tag == "healing")
        {
            Rightlegisbroken = false;
            RightLegDamage -= 1;
            Debug.Log("Right leg healed" + RightLegDamage);
        }
        if (Leftarmisbroken && collision.gameObject.tag == "healing")
        {
            Leftarmisbroken = false;
            RightArmDamage -= 1;
            Debug.Log("Left arm healed" + LeftArmDamage);
        }
        if (Rightarmisbroken && collision.gameObject.tag =="healing")
        {
            Rightarmisbroken = false;
            RightArmDamage -= 1;
            Debug.Log("Right arm healed" + RightArmDamage);
        }
    }
}