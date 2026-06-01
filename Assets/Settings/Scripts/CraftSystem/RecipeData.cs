using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/Recipe")]
public class RecipeData : ScriptableObject
{
    [SerializeField] public ItemData[] ingredients; // benodigde items
    [SerializeField] public ItemData result;    // wat je krijgt na craft
}
