    using UnityEngine;

[CreateAssetMenu (menuName ="Crafting/Item")]
public class ItemData : ScriptableObject
{
    public GameObject prefab;
    [SerializeField] public  string itemName;
    [SerializeField] public Sprite icon;
}
