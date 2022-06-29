using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

namespace MainMode.GameMechanics
{
    public class PlayGround : MonoBehaviour
    {
        public static PlayGround Instance;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private List<Vector3Int> _occupiedCells = new List<Vector3Int>();

        private Vector3Int _invalidValue = new Vector3Int(-1000, -1000, 0);
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }else if(Instance == this)
            {
                Destroy(gameObject);    
            }
        }
        private void Start()
        {
            FindStartDevices();

        }

        public Vector3Int GetRandomFreeCell(Vector3 playerPosition, float radius)
        {
            var neighbours = GetNeighbours(playerPosition, radius);

            var freeNeighbours = neighbours.Except(_occupiedCells).ToList();

            Vector3Int randomFreeCell;
            if (freeNeighbours.Count > 0)
            {
                randomFreeCell = freeNeighbours[Random.Range(0, freeNeighbours.Count)];
                _occupiedCells.Add(randomFreeCell);

            }
            else
                randomFreeCell = _invalidValue;

            return randomFreeCell;
        }

        public bool TryGetFreeRandomCell(Vector3 playerPosition, float radius, out Vector3Int randomFreeCell)
        {
            var neighbours = GetNeighbours(playerPosition, radius);

            var freeNeighbours = neighbours.Except(_occupiedCells).ToList();

            randomFreeCell = _invalidValue;

            if (freeNeighbours.Count > 0)
            {
                randomFreeCell = freeNeighbours[Random.Range(0, freeNeighbours.Count)];
                _occupiedCells.Add(randomFreeCell);
                return true;

            }
            else
                return false;

        }

        public void DeleteDeviceOnCell(Vector3Int cell)
        {
            foreach (var item in _occupiedCells)
            {
                if (item.Equals(cell))
                {
                    _occupiedCells.Remove(item);
                    break;
                }
            }
        }


        private List<Vector3Int> GetNeighbours(Vector3 playerPosition, float radius)
        {
            var cellPlayer = _tilemap.WorldToCell(playerPosition);
            var neighbours = new List<Vector3Int>();

            for (int x = -(int)radius; x <= radius; x++)
            {
                for (int y = -(int)radius; y <= radius; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = cellPlayer.x + x;
                    int checkY = cellPlayer.y + y;

                    if (checkX >= 0 && checkX <= 19 && checkY >= 0 && checkY <= 19)
                    {
                        neighbours.Add(new Vector3Int(checkX, checkY, 0));
                    }
                }
            }

            return neighbours;
        }

        private void FindStartDevices()
        {
            var trapsOnScene = FindObjectsOfType<Trap>();
            foreach (var trap in trapsOnScene)
            {
                trap.CellPos = _tilemap.WorldToCell(trap.transform.position);
                _occupiedCells.Add(_tilemap.WorldToCell(trap.transform.position));
            }

            var izolatorsOnScene = FindObjectsOfType<Izolator>();
            foreach (var izolator in izolatorsOnScene)
            {
                izolator.CellPos = _tilemap.WorldToCell(izolator.transform.position);
                _occupiedCells.Add(_tilemap.WorldToCell(izolator.transform.position));
            }

            var gunsOnScene = FindObjectsOfType<Gun>();
            foreach (var gun in gunsOnScene)
            {
                gun.CellPos = _tilemap.WorldToCell(gun.transform.position);
                _occupiedCells.Add(_tilemap.WorldToCell(gun.transform.position));
            }
        }
    }
}