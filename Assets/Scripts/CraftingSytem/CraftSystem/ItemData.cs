    using UnityEngine;

[CreateAssetMenu (menuName ="Crafting/Item")]
public class ItemData : ScriptableObject
{
    public GameObject prefab;
    public  string itemName;
    public Sprite icon;
}
