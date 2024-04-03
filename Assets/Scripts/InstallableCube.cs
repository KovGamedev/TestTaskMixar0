using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshRenderer))]
public class InstallableCube : MonoBehaviour
{
    private MeshRenderer _renderer;

    public void ChangeColor()
    {
        _renderer.material.color = Random.ColorHSV();
    }

    public void OnMouseDown()
    {
        Debug.Log(123);
    }

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }
}
