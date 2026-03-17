using UnityEngine;

[CreateAssetMenu(fileName = "ItemActivatorData", menuName = "Scriptable Objects/ItemActivatorData")]
public class ItemActivatorData : ScriptableObject
{
    [Header("Requirements & Rewards")]
    public string itemName;            // The ID string to check for
    public GameObject RewardItem;      // The prefab to spawn

    [Header("Effects")]
    public GameObject itemActivatorEffect; // Particle prefab
    public AudioClip interactionSound;     // Use AudioClip, not AudioSource

    [Header("UI & Settings")]
    public string uiMessage = "Press E to Open";
    public int UIRange = 3;
}