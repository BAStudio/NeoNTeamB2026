using UnityEngine;

using UnityEngine.UI;

using UnityEngine.Rendering;

using UnityEngine.Rendering.Universal;



public class BodyVisualManager : DamageBase

{

    public Image bodyImage;
    public Image leftArmImage;
    public Image rightArmImage;
    public Image LeftLegImage;
    public Image RightLegImage;
    public Image HeadImage;

    public float maxDamageBeforeBroken = 3f; // The "Normal" threshold
    public Color healthyColor = Color.green;
    public Color brokenColor = Color.red;
    public Volume Vignette;

    private void Updatepart(Image img, float currentDamage)

    {

        if (img == null) return;
        float damagePercentage = Mathf.Clamp01(currentDamage / Mathf.Max(maxDamageBeforeBroken, 0.0001f));
        img.color = Color.Lerp(healthyColor, brokenColor, damagePercentage);

    private void UpdateVignette()

    {

      if (Vignette == null) return;
      float damagePercentage = Mathf.Clamp01(FullyBodyHealth / 10f);
      Vignette.weight = damagePercentage;

    }
    // Update is called once per frame

    void Update()
    {
        Updatepart(leftArmImage, LeftArmDamage);
        Updatepart(rightArmImage, RightArmDamage);
        Updatepart(LeftLegImage, LeftLegDamage);
        Updatepart(RightLegImage, RightLegDamage);
        Updatepart(bodyImage, TorsoDamage);
        Updatepart(HeadImage, HeadDamage);
        UpdateVignette();
    }

}