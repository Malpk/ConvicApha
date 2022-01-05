using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TileSpace;

public class InstateTileInRadius : MonoBehaviour
{
    [Header("Game Setting")]
    [SerializeField] private int _radius;
    [SerializeField] private float _delay;
    [SerializeField] private float _duratuinOneRound;
    [SerializeField] private UnderworldTile _underworldTile;

    [Inject] private Transform _player;
    [Inject] private GameMap _map;

    private IGetTileMap _tiles;

    private void Awake()
    {
        _tiles = _underworldTile;
    }

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        float progress = 0;
        while (progress <1)
        {
            List<IVertex> list = _map.GetVertexInRadius(_player.position, _radius);
            if (list.Count > 0)
            {
                int index = Random.Range(0, list.Count);
                var tile = Instantiate(_tiles.GetTypeTile(), list[index].VertixPosition, Quaternion.identity);
                list[index].SetTile(tile);
                yield return new WaitForSeconds(_delay);
                progress += _delay / _duratuinOneRound;
            }
            else
            {
                yield return null;
            }
        }
        Destroy(gameObject);
        yield return null;
    }
}
