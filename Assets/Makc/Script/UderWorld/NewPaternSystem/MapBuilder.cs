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
        public void Intialiate(GameMap map)
        {
            _pointsMap = Intializate(map.Map);
        }
        public void OnDestroy()
        {
            foreach (var point in _pointsMap)
            {
                point.DestroyObject();
            }
        }
        private Point[,] Intializate(IVertex[,] tileMap)
        {
            var countLine = tileMap.GetLength(0);
            var countColumen = tileMap.GetLength(1);
            var points = new Point[countColumen, countLine];
            for (int i = 0; i < countLine; i++)
            {
                for (int j = 0; j < countColumen; j++)
                {
                    points[i, j] = new Point(tileMap[j, i].VertixPosition);
                    points[i, j].CreateObject(_tileTern);
                    points[i, j].SetAtiveObject(false);
                }
            }
            return points;
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
            Debug.Log("sss");
            foreach (var point in _pointsMap)
            {
                point.Animation.Stop();
                lost = point;
            }
            return lost;
        }
    }
}