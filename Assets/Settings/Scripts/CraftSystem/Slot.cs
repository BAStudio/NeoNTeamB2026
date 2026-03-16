using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour 
{
    public ItemData item;
    [SerializeField] private Image icon;

    public void SetItem(ItemData newItem)
    {
        item = newItem;

        if (icon == null)
        {
            Debug.LogError("Icon is niet gekoppeld in " + gameObject.name);
            return;
        }

        icon.sprite = newItem.icon;
        icon.enabled = true;
    }

    public void Clear()
    {
        item = null;

        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }
}



