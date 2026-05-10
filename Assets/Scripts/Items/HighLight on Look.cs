using UnityEngine;
using UnityEngine.UIElements;

public class HighLightonLook : MonoBehaviour
{
    private Renderer _renderer;
    private Material[] _materials;
    public int indexOfMat = 1;

    private void Awake()
    {
        Debug.Log("[OUTLINE] Awake: " + gameObject.name);
        _renderer = GetComponent<Renderer>();
        // Cache the materials array
        _materials = _renderer.materials;
    }

    public void Outline(bool show)
    {
        float value = show ? 1f : 0f;
        Debug.Log($"[OUTLINE] Setting {gameObject.name} to {value}");

        _materials[indexOfMat].SetFloat("_Outline", value);
        _renderer.materials = _materials;
    }
  
}
