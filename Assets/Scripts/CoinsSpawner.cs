using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private int _coinsQuantity;
    [SerializeField] private CoinsCounter _coinsCounter;

    private void Start()
    {
        SpawnCoins();
    }

    private void SpawnCoins()
    {
        for (var i = 0; i < _coinsQuantity; i++)
        {
            var randomInCircle = Random.insideUnitCircle;
            var randomPosition = _spawnRadius * new Vector3(randomInCircle.x, 0, randomInCircle.y);
            var coin = Instantiate(_coinPrefab, randomPosition, Quaternion.identity).GetComponent<Coin>();
            coin.SetCoinsCounter(_coinsCounter);
        }
    }
}
