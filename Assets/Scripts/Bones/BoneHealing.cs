using UnityEngine;

public class BoneHealing : DamageBase
{
    // References to limb damage components used for manual healing via key presses
    [SerializeField] private RightArmDamage rightArmDamage;
    [SerializeField] private LeftArmDamage leftArmDamage;
    [SerializeField] private RightLegDamage rightLegDamage;
    [SerializeField] private LeftLegDamage leftLegDamage;

    private void Start()
    {
        if (playerMovement == null) playerMovement = FindObjectOfType<PlayerMovement>();
        if (rightArmDamage == null) rightArmDamage = FindObjectOfType<RightArmDamage>();
        if (leftArmDamage == null) leftArmDamage = FindObjectOfType<LeftArmDamage>();
        if (rightLegDamage == null) rightLegDamage = FindObjectOfType<RightLegDamage>();
        if (leftLegDamage == null) leftLegDamage = FindObjectOfType<LeftLegDamage>();


    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            // Heal right arm when pressing Y
            if (rightArmDamage != null)
            {
                rightArmDamage.RightArmHealing();
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            // Heal left arm when pressing U
            if (leftArmDamage != null)
            {
                leftArmDamage.LeftArmHealing();
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            // Heal right leg when pressing H
            if (rightLegDamage != null)
            {
                rightLegDamage.RightLegHealed();
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Heal left leg when pressing J
            if (leftLegDamage != null)
            {
                leftLegDamage.HealLeftLeg();
            }
        }
    }
}