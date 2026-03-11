using System;
using UnityEngine;

public class Slot : Item
{
    [SerializeField] public Item  item;

    [SerializeField] private int index; // Het slotnummer (bijv 0,1,2,3)
    public int Index => index; // Andere scripts kunnen index lezen maar niet veranderen
}



