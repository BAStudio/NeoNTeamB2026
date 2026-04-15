using UnityEngine;

public class HighLightonLook : MonoBehaviour
{
    public Material highlightMaterial; // The material to use for highlighting
    public int indexOfMat = 1;

    private void Awake()
    {
        highlightMaterial = GetComponent<Renderer>().materials[indexOfMat];
    }

    public void Outline(bool show)
    {
        highlightMaterial.SetFloat("_Outline", show ? 1f : 0f);
    }
  
}
