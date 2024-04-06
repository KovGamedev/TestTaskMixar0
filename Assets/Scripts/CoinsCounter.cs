using TMPro;
using UnityEngine;

public class CoinsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsCounter;

    private int _points = 0;

    public void AddPoint()
    {
        _points++;
        _coinsCounter.text = _points.ToString();
    }
}
