using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float _animationAltitude;
    [SerializeField] float _animationDuration;
    [SerializeField] float _animationRotationSpeed;

    private Transform _model;
    private Vector3 _animationInitialPosition;
    private CoinsCounter _coinsCounter;

    public void SetCoinsCounter(CoinsCounter coinsCounter)
    {
        _coinsCounter = coinsCounter;
    }

    public void Collect()
    {
        _coinsCounter.AddPoint();
        Destroy(gameObject);
    }

    private void Awake()
    {
        _model = transform.GetChild(0);
        _animationInitialPosition = _model.localPosition;
    }

    private void Start()
    {
        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        var targetPosition = _animationInitialPosition + _animationAltitude * Vector3.up;
        while (true) {
            var animatinIterationDuration = 0f;
            while (animatinIterationDuration < _animationDuration) {
                _model.localPosition = Vector3.Lerp(_animationInitialPosition, targetPosition, animatinIterationDuration / _animationDuration);
                animatinIterationDuration = Mathf.Clamp(animatinIterationDuration + Time.fixedDeltaTime, 0, _animationDuration);
                yield return null;
            }
            animatinIterationDuration = 0f;
            while (animatinIterationDuration < _animationDuration)
            {
                _model.localPosition = Vector3.Lerp(targetPosition, _animationInitialPosition, animatinIterationDuration / _animationDuration);
                animatinIterationDuration = Mathf.Clamp(animatinIterationDuration + Time.fixedDeltaTime, 0, _animationDuration);
                yield return null;
            }
        }
    }

    private void FixedUpdate()
    {
        _model.Rotate(0, _animationRotationSpeed, 0);
    }
}
