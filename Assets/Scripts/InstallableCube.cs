using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class InstallableCube : MonoBehaviour
{
    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    public void ChangeColor()
    {
        _renderer.material.color = Random.ColorHSV();
    }
}
