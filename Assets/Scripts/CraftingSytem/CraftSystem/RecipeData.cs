using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/Recipe")]
public class RecipeData : ScriptableObject
{
    public ItemData[] ingredients; // benodigde items
     public ItemData result;    // wat je krijgt na craft
}
