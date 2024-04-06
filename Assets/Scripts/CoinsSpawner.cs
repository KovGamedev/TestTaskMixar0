using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] private ARPlaneManager _planeManager;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private int _coinsQuantity;
    [SerializeField] private CoinsCounter _coinsCounter;

    private List<Transform> _coins = new();

    private void Start()
    {
        _planeManager.planesChanged += SpawnCoins;
        _planeManager.planesChanged += UpdateCoins;
    }

    private void SpawnCoins(ARPlanesChangedEventArgs changes)
    {
        if (0 < changes.added.Count)
        {
            var plane = changes.added[0];
            for (var i = 0; i < _coinsQuantity; i++)
            {
                var randomInCircle = Random.insideUnitCircle;
                var randomPosition = _spawnRadius * new Vector3(randomInCircle.x, plane.transform.position.y, randomInCircle.y);
                var coin = Instantiate(_coinPrefab, randomPosition, Quaternion.identity).GetComponent<Coin>();
                coin.SetCoinsCounter(_coinsCounter);
                _coins.Add(coin.transform);
            }
            _planeManager.planesChanged -= SpawnCoins;
        }
    }

    private void UpdateCoins(ARPlanesChangedEventArgs changes)
    {
        if (0 < changes.updated.Count)
        {
            var plane = changes.updated[0];
            foreach (var coin in _coins)
            {
                coin.position = new Vector3(coin.position.x, plane.transform.position.y, coin.position.z);
            }
            _planeManager.planesChanged -= SpawnCoins;
        }
    }

    private void OnDestroy()
    {
        _planeManager.planesChanged -= SpawnCoins;
        _planeManager.planesChanged -= UpdateCoins;
    }
}
