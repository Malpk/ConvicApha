using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileSpace;
using SwitchModeComponent;

public class InstateTileInRadius : MonoBehaviour, IModeForSwitch
{
    [Header("Game Setting")]
    [SerializeField] private int _radius;
    [SerializeField] private float _delay;
    [SerializeField] private float _duratuinOneRound;
    [SerializeField] private UnderworldTile _underworldTile;

    private GameMap _map;
    private Coroutine _coroutine;
    private Transform _player;

    private UnderworldTile _tiles;

    public bool IsAttackMode => true;

    private void Awake()
    {
        _tiles = _underworldTile;
    }
    public void Constructor(SwitchMode swictMode)
    {
        if (_coroutine != null)
            return;
        _player = swictMode.playerTransform;
        _map = swictMode.map;
        _coroutine = StartCoroutine(Spawn());
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
                tile.transform.parent = transform.parent;
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
