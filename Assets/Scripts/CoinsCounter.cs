using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(AudioSource))]
public class CoinsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsCounter;

    private AudioSource _audioSource;
    private int _points = 0;

    public void AddPoint()
    {
        _points++;
        _coinsCounter.text = _points.ToString();
        _audioSource.Play();
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}
