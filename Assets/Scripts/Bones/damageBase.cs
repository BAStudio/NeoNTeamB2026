using UnityEngine;

public class DamageBase : MonoBehaviour
{
    protected static float RightLegDamage = 0;
    protected float LeftLegDamage = 0;
    protected static float LeftArmDamage = 0;
    protected static float RightArmDamage = 0;
    protected static float Health = 0;

    protected static bool Leftlegisbroken = false;
    protected static bool Rightlegisbroken = false;
    protected static bool Leftarmisbroken = false;
    protected static bool Rightarmisbroken = false;

    [SerializeField] protected PlayerMovement playerMovement;
}