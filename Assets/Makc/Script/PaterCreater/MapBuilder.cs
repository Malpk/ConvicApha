using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Underworld
{
    [System.Serializable]
    public class MapBuilder
    {
        [Header("Map Setting")]
        [Min(1)]
        [SerializeField] protected int mapSize = 1;
        [SerializeField] protected Vector2 unitSize;
        [SerializeField] protected GameObject tileTern;

        private Point[,] _pointsMap;
        
        public Vector2 UnitSize => unitSize;
        public Point[,] Map => _pointsMap;
        
        public void OnDestroy()
        {
            foreach (var point in _pointsMap)
            {
                point.DestroyObject();
            }
            _pointsMap = null;
        }
        public bool Intializate(Transform parent = null)
        {
            if (_pointsMap != null)
                return false;
            CheakValue();
            parent = GetHolder("PointHolder", parent);
            var radius = mapSize / 2 - 1;
            var startPosition = new Vector2(-unitSize.x / 2 - unitSize.x * radius,
                unitSize.y / 2 + unitSize.y * radius);
            _pointsMap = new Point[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    _pointsMap[i, j] = new Point(startPosition + new Vector2(unitSize.x* j,-unitSize.y * i));
                    if (tileTern != null)
                    {
                        _pointsMap[i, j].CreateObject(tileTern).transform.parent = parent;
                        _pointsMap[i, j].SetAtiveObject(false);
                    }
                }
            }
            if (tileTern == null)
                Debug.LogWarning("Points is Empty");
            return true;
        }
        private void CheakValue()
        {
            if (mapSize <= 0)
                throw new System.Exception("Map Size is can't be <= 0");
            if(unitSize == Vector2.zero || unitSize.x < 0 || unitSize.y < 0)
                throw new System.Exception("UnitSize Size is can't be <= 0");
        }
        private Transform GetHolder(string name, Transform parent)
        {
            var holder = new GameObject("PointHolder").transform;
            holder.parent = parent;
            holder.localPosition = Vector2.zero;
            return holder;
        }
        public void MapUpdate(TermMode[,] paternMap)
        {
            for (int i = 0; i < paternMap.GetLength(0); i++)
            {
                for (int j = 0; j < paternMap.GetLength(1); j++)
                {
                    if (paternMap[i, j] != TermMode.Deactive)
                    {
                        ChooseMode(_pointsMap[i, j], paternMap[i, j]);
                    }
                    else
                    {
                        _pointsMap[i, j].SetAtiveObject(false);
                    }
                }
            }
        }
        private void ChooseMode(Point point,TermMode mode)
        {

            point.SetAtiveObject(true);
            switch (mode)
            {
                case TermMode.SafeMode:
                    break;
                case TermMode.AttackMode:
                    point.Animation.StartTile();
                    break;
            }
        }
        public Point TurnOffAllTile()
        {
            Point lost = null;
            foreach (var point in _pointsMap)
            {
                point.Animation.Stop();
                lost = point;
            }
            return lost;
        }
    }
}