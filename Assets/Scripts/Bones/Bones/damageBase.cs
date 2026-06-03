using UnityEngine;

public class DamageBase : MonoBehaviour
{
    protected float RightLegDamage = 0f;
    protected float LeftLegDamage = 0f;
    protected float LeftArmDamage = 0f;
    protected float RightArmDamage = 0f;
    protected static float FullyBodyHealth = 100f;
    protected static float Health = 0;

    protected bool Leftlegisbroken = false;
    protected bool Rightlegisbroken = false;
    protected bool Leftarmisbroken = false;
    protected bool Rightarmisbroken = false;

    [SerializeField] protected PlayerMovement playerMovement;
}