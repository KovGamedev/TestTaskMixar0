using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class InstallableCube : MonoBehaviour
{
    [SerializeField] private float _movingSpeed;

    private MeshRenderer _renderer;
    private float _directionSign;

    public void ChangeColor()
    {
        _renderer.material.color = Random.ColorHSV();
    }

    public void StartMoveRight()
    {
        _directionSign = 1;
    }

    public void StartMoveLeft()
    {
        _directionSign = -1;
    }

    public void StopMove()
    {
        _directionSign = 0;
    }

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        if (_directionSign != 0)
            transform.position += _directionSign * _movingSpeed * Camera.main.transform.right;
    }
}
