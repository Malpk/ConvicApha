using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public abstract class PaternMode : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] protected bool playOnStart;
        [Header("Requred Reference")]
        [SerializeField] protected Player player;
        [SerializeField] protected MapBuilder builder;

        protected Point[,] map;
        protected Vector2Int mapSize;
        protected Vector2Int centerPosition;

        private void Awake()
        {
            if (builder)
                CreateMap(builder);
        }
        public void Intializate(MapBuilder builder, Player player)
        {
            this.builder = builder;
            this.player = player;
            CreateMap(builder);
        }
        public abstract void Run();
        protected void CreateMap(MapBuilder builder)
        {
            if (map != null)
            {
                foreach (var point in map)
                {
                    point.Clear();
                }
            }
            map = builder.Map;
            mapSize = new Vector2Int(map.GetLength(0), map.GetLength(1));
            centerPosition = mapSize / 2;
        }
    }
}
