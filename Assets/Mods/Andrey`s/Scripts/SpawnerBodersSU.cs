using UnityEngine;

public class SpawnerBodersSU : MonoBehaviour
{
    [SerializeField] private float _spawnTime = 3f;
    [SerializeField] private GameObject[] _groups;
    [SerializeField] private GameObject _upRightPoint;
    [SerializeField] private GameObject _downLeftPoint;

    private float _progress = 0f;

    void Update()
    {
        _progress += Time.deltaTime / _spawnTime;
        if (_progress >= 1)
        {
            var x = Random.Range(_downLeftPoint.transform.position.x,
                _upRightPoint.transform.position.x);
            var y = Random.Range(_downLeftPoint.transform.position.y,
                _upRightPoint.transform.position.y);
            Instantiate(_groups[Random.Range(0, _groups.Length)], new Vector2(x,y), transform.rotation);
            _progress = 0f;
        }
    }
}
