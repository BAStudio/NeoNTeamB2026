using UnityEngine;

public class boneHealing : DamageBase
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "healing")
        { 
            if (!collision.gameObject.CompareTag("healing"))
                return;

            Debug.Log("boneHealing: found healing tag on " + collision.gameObject.name);

            if (Leftlegisbroken)
            {
                Leftlegisbroken = false;
                LeftLegDamage = Mathf.Max(0, LeftLegDamage - 1);
                Debug.Log("Left leg healed " + LeftLegDamage);
            }

            if (Rightlegisbroken)
            {
                Rightlegisbroken = false;
                RightLegDamage = Mathf.Max(0, RightLegDamage - 1);
                Debug.Log("Right leg healed " + RightLegDamage);
            }

            if (Leftarmisbroken)
            {
                Leftarmisbroken = false;
                LeftArmDamage = Mathf.Max(0, LeftArmDamage - 1);
                Debug.Log("Left arm healed " + LeftArmDamage);
            }

            if (Rightarmisbroken)
            {
                Rightarmisbroken = false;
                RightArmDamage = Mathf.Max(0, RightArmDamage - 1);
                Debug.Log("Right arm healed " + RightArmDamage);
            }

            
        }
    }
}