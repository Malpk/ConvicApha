using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TileSpace;
using SwitchMode;

public class FireMap : MonoBehaviour, ISequence
{
    [SerializeField] private int _countIsland;
    [SerializeField] private int _maxCountTile;
    [SerializeField] private GameObject _tile;

    private GameMap _map = null;

    private List<IVertex> _allMap;
    private IVertex[,] _vertexs;

    public bool IsAttackMode => true;

    public void Constructor(SwitchMods swictMode)
    {
        if (_map == null)
        {
            _map = swictMode.map;
            StartCoroutine(ModeRun());
        }
    }

    private IEnumerator ModeRun()
    {
        var mapSpawn = GetMapSpawn();
        GameObject endTile = null;
        foreach (var vertex in mapSpawn)
        {
            endTile = Instantiate(_tile, vertex.VertixPosition, Quaternion.identity);
            endTile.transform.parent = transform; 
        }
        yield return new WaitWhile(() => (endTile!=null));
        Destroy(gameObject);
    }

    private List<IVertex> GetMapSpawn()
    {
        _vertexs = _map.Map;
        _allMap = GetVertexList(_vertexs);
        var map = GetVertexList(_vertexs);
        var seed = GetSeeds(map);
        seed = SetIslandSize(seed);
        foreach (var item in seed)
        {
            _allMap.Remove(item);
        }
        return _allMap;
    }

    private List<IVertex> GetSeeds(List<IVertex> list)
    {
        List<IVertex> seeds = new List<IVertex>();
        int interval = Mathf.RoundToInt(_map.MapSize / Mathf.Sqrt(_countIsland));
        for (int i = 0; i < _countIsland && list.Count > 0; i++)
        {
            int index = Random.Range(0, list.Count);
            int curretInterval = Random.Range(interval - 1, interval);
            seeds.Add(list[index]);
            list = CutRadius(list[index], list, _vertexs, curretInterval);
        }
        return seeds;
    }
    private List<IVertex> CutRadius(IVertex chooseVertex, List<IVertex> mapList,IVertex [,] vertexs,int interval)
    {
        interval /= 2;
        var index = chooseVertex.ArryaPostion;
        for (int i =-interval; i <= interval; i++)
        {
            int x = Mathf.Clamp(index.x + i, 0 ,_map.MapSize-1);
            for (int j = -interval; j <= interval; j++)
            {
                int y = Mathf.Clamp(index.y + j, 0, _map.MapSize-1);
                mapList.Remove(vertexs[x,y]);
            }
        }
        return mapList;
    }
    private List<IVertex> GetVertexList(IVertex[,] vertexs)
    {
        List<IVertex> map = new List<IVertex>();
        for (int i = 0; i < _map.MapSize; i++)
        {
            for (int j = 0; j < _map.MapSize; j++)
            {
                if(vertexs[i, j].State == TileState.Empty)
                    map.Add(vertexs[i, j]);
            }
        }
        return map;
    }
    private List<IVertex> SetIslandSize(List<IVertex> seeds)
    {
        List<IVertex> islandsOnMap = new List<IVertex>();
        int count = seeds.Count;
        for (int i = 0; i < count; i++)
        {
            List<IVertex> island = new List<IVertex>();
            List<IVertex> accessForExtension = new List<IVertex>();
            island.Add(seeds[i]);
            int countTile = Random.Range(1, _maxCountTile);
            for (int j = 0; j < countTile; j++)
            {
                int seedsID = Random.Range(0, island.Count);
                foreach (var vertex in island[seedsID].Edge)
                {
                    accessForExtension.Add(vertex);
                }
                int index = Random.Range(0, accessForExtension.Count);
                island.Add(accessForExtension[index]);
                accessForExtension.Remove(accessForExtension[index]);
            }
            islandsOnMap.AddRange(island);
        }
        return islandsOnMap;
    }


}
