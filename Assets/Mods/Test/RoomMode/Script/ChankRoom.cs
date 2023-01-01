using UnityEngine;
using System.Collections.Generic;

namespace MainMode.Room
{
    public class ChankRoom : MonoBehaviour
    {
        [SerializeField] private Vector2 _unit = Vector2.one;
        [SerializeField] private Vector2Int _maxSizeMap = new Vector2Int(20, 20);
        [Header("Reference")]
        [SerializeField] private Room[] _roomPrefabs;

        private bool[,] _map;
        private List<Room> _rooms = new List<Room>();


        void Start()
        {
        }

        public void GenerateMap()
        {
            
        }

        private Room GetRoomPrefab()
        {
            return _roomPrefabs[Random.Range(0, _roomPrefabs.Length)];
        }
    }
}