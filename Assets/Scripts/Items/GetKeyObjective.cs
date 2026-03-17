using UnityEngine;

public class GetKeyObjective : MonoBehaviour
{
    [SerializeField] GameObject Chest;
    private static bool isKeyObtained = false;


    // Update is called once per frame
    void Start()
    {
        isKeyObtained = false;
        
    }

    private void OnTriggerStay(Collider other)
    {
       if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isKeyObtained = true;
                Chest.SetActive(false);
            }
        }
    }
}
