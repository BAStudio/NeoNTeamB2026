using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UiInteract : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Transform camera;

    [SerializeField] LayerMask layerMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckObject();  
    }

    public void EnableText()
    {
        text.gameObject.SetActive(true);    
    }

    private void DisableText()
    {
        text.gameObject.SetActive(false);
    }

    private void CheckObject()
    {
        RaycastHit hit;

        if (Physics.Raycast(camera.transform.position, camera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
           
                text.gameObject.SetActive(true);


            
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }

}

