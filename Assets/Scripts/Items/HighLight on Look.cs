using UnityEngine;
using UnityEngine.UIElements;

public class HighLightonLook : MonoBehaviour
{
    private Renderer _renderer;
    private Material[] _materials;
    public int indexOfMat = 1;
    bool isHighlighted = false;


    private void Awake()
    {
        Debug.Log("[OUTLINE] Awake: " + gameObject.name);
        _renderer = GetComponent<Renderer>();
        // Cache the materials array
        _materials = _renderer.materials;
    }

    public void Outline(bool show)
    {
        float Thickness = show ? 0.02f : 0f;
        Debug.Log($"[OUTLINE] Setting {gameObject.name} to {Thickness}");

        _materials[indexOfMat].SetFloat("_Thickness", Thickness);
        _materials[indexOfMat].SetFloat("_ShowOutline", show ? 1f : 0f);
        _renderer.materials = _materials;
    }
  
}
