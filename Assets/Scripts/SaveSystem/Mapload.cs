using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Mapload : MonoBehaviour
{
    [SerializeField]private List <GameObject>List1 = new List<GameObject>();
    [SerializeField]private List <GameObject>List2 = new List<GameObject>();

    bool listof2isoff = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collided();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            collided();
        }
    }


   private void collided()
    {
        if (listof2isoff)
        {
            foreach (GameObject obj in List1)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in List2)
            {
                obj.SetActive(true);
            }
            listof2isoff = false;
        }
            else
            {
                foreach (GameObject obj in List1)
                {
                    obj.SetActive(true);
                }
                foreach (GameObject obj in List2)
                {
                    obj.SetActive(false);
                }
                listof2isoff = true;
        }
    }
}
