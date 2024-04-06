using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class InstallableCube : MonoBehaviour
{
    [SerializeField] private float _movingSpeed;
    [SerializeField] private float _speedMultiplier;

    private MeshRenderer _renderer;
    private float _directionSign;
    private ImagesTracker _imagesTracker;
    private float _currentMultiplier = 1f;

    public void ChangeColor()
    {
        _renderer.material.color = Random.ColorHSV();
    }

    public void SetImageTracker(ImagesTracker imagesTracker)
    {
        _imagesTracker = imagesTracker;
        if (_imagesTracker.WasImageRecognized())
            IncreaseMultiplier();
        else
            _imagesTracker.GetImageRecognitionEvent().AddListener(IncreaseMultiplier);
    }

    private void IncreaseMultiplier()
    {
        _currentMultiplier = _speedMultiplier;
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
            transform.position += _directionSign * _movingSpeed * _currentMultiplier * Camera.main.transform.right;
    }

    private void OnDestroy()
    {
        _imagesTracker.GetImageRecognitionEvent().RemoveListener(IncreaseMultiplier);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<Coin>(out var coin))
            coin.Collect();
    }
}
