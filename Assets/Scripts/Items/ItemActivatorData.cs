using UnityEngine;

[CreateAssetMenu(fileName = "ItemActivatorData", menuName = "Scriptable Objects/ItemActivatorData")]
public class ItemActivatorData : ScriptableObject
{
    [Header("Requirements & Rewards")]
    public string itemName;            
    public GameObject RewardItem;      

    [Header("Effects")]
    public GameObject itemActivatorEffect;
    public AudioClip interactionSound;     

    [Header("UI & Settings")]
    public string uiMessage = "Press E to Open";
    public int UIRange = 3;
}