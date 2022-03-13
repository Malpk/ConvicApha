using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TileSpace;

namespace Underworld
{
    [System.Serializable]
    public class MapBuilder
    {
        [SerializeField] protected GameObject _tileTern;

        private Point[,] _pointsMap;
        public Point[,] Map => _pointsMap;

        public void OnDestroy()
        {
            foreach (var point in _pointsMap)
            {
                point.DestroyObject();
            }
            _pointsMap = null;
        }
        public bool Intializate(Vector2 unitSize, int size ,Transform parent = null)
        {
            if (_pointsMap != null)
                return false;
            parent = GetHolder("PointHolder", parent);
            var radius = size / 2 - 1;
            var startPosition = new Vector2(-unitSize.x / 2 - unitSize.x * radius,
                unitSize.y / 2 + unitSize.y * radius);
            _pointsMap = new Point[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _pointsMap[i, j] = new Point(startPosition +
                        new Vector2(unitSize.x* j,-unitSize.y * i));
                    _pointsMap[i, j].CreateObject(_tileTern).transform.parent = parent;
                    _pointsMap[i, j].SetAtiveObject(false);
                }
            }
            return true;
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