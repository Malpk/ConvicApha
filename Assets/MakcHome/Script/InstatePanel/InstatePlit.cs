using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InstatePlit : MonoBehaviour
{
    [SerializeField] private int _radius;
    [SerializeField] private float _delay;
    [SerializeField] private float _duratuinOneRound;
    [SerializeField] private float _durationTwoMode;
    [SerializeField] private Transform _player;
    [SerializeField] private UnderworldTile _underworldTile;

    [Inject] private GameMap _map;
    [Inject] private FireMap _fireMap;

    private IGetTileMap _tiles;

    public delegate void ModeActive(UnderWorldMode mode);
    public event ModeActive ModeActiveAction;

    private void Awake()
    {
        _tiles = _underworldTile;
    }

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator FireToMap()
    {
        Invoke(UnderWorldMode.MapFIre);
        var mapSpawn = _fireMap.GetMapSpawn();
        foreach (var vertex in mapSpawn)
        {
            Instantiate(_tiles.GetTypeTile(), vertex.VertixPosition, Quaternion.identity);
        }
        yield return new WaitForSeconds(_durationTwoMode);
        StartCoroutine(Spawn());
        yield return null;
    }

    IEnumerator Spawn()
    {
        float progress = 0;
        Invoke(UnderWorldMode.OneFire);
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
        StartCoroutine(FireToMap());
        yield return null;
    }
    private void Invoke(UnderWorldMode mode)
    {
        if (ModeActiveAction != null)
            ModeActiveAction(mode);
    }
}
