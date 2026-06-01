    using UnityEngine;

[CreateAssetMenu (menuName ="Crafting/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] public  string itemName;
    [SerializeField] public Sprite icon;
}
