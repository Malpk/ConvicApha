using MainMode.GameMechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MainMode
{
    public class RandomSpawner : MonoBehaviour
    {
        [Min(1)]
        [SerializeField] private float _radiusSpawnZone;
        [SerializeField] private float _spanwGap;
        [Min(0)]
        [SerializeField] private float _spawnTimer;
        [SerializeField] private Player _player;
        [SerializeField] private KIFactory _kiFactory;
        [SerializeField] private AnimationCurve _difficultCurve;
        [SerializeField] private PlayGround _playGround;
        [SerializeField]
        [Range(2, 8)]
        private int limitNeighbours = 3;
        private Vector3 _offset = new Vector3(0.5f, 0.5f, 0);

        private Dictionary<TrapType, int[]> _spwnZones = new Dictionary<TrapType, int[]>()
        {
            [TrapType.N7] = new int[2] { 1, 3 },
            [TrapType.TI81] = new int[2] { 1, 3 },
            [TrapType.U92] = new int[2] { 1, 3 },
            [TrapType.C14] = new int[2] { 1, 3 },
            [TrapType.Turel] = new int[2] { 7, 8 },
            [TrapType.FireGun] = new int[2] { 7, 8 },
            [TrapType.LaserGun] = new int[2] { 7, 8 },
            [TrapType.VenomIsolator] = new int[2] { 4, 6 },
            [TrapType.SteamIsolator] = new int[2] { 4, 6 },
            [TrapType.RocketLauncher] = new int[2] { 7, 8 },
            [TrapType.DaimondIzolator] = new int[2] { 4, 6 }
        };
    
        private Vector3Int spawnCell;
        private List<Vector3Int> freeCells;
        private bool isTimerActive = false; 

        private void Update()
        {
            if (!isTimerActive)
                return;
            _spanwGap = _difficultCurve.Evaluate(_spawnTimer);
            _spawnTimer += Time.deltaTime;

        }
        private IEnumerator DebugSpawn()
        {
            while (true)
            {
                bool spawned = false;
                var device = _kiFactory.GetRandomKI() as Device;
                device.gameObject.SetActive(false);

                while (spawned == false)
                {
                    if (_playGround.TryGetFreeCellsInRange(_player.transform.position, _spwnZones[device.DeviceType], out freeCells))
                    {

                        while (freeCells.Count > 0)
                        {
                            spawnCell = freeCells[Random.Range(0, freeCells.Count)];
                            var countFreeCells = _playGround.GetCountFreeCellsAroundCell(spawnCell);
                            int countNeighbours = 8 - countFreeCells;

                            if (countNeighbours < limitNeighbours)
                            {
                                device.gameObject.SetActive(true);
                                device.transform.position = _playGround.GetWorldPositionByCellPos(spawnCell) + _offset;
                                device.CellPos = spawnCell;
                                _playGround.OccupyCell(spawnCell);
                                spawned = true;
                                break;
                            }
                            else
                            {
                                freeCells.Remove(spawnCell);
                            }
                        }

                        if (freeCells.Count == 0)
                        {
                            Destroy(device.gameObject);
                            break;

                        }

                    }
                    else
                    {
                        Destroy(device.gameObject);
                        break;
                    }
                }          
            
                yield return new WaitForSeconds(_spanwGap);
            }
        }

        private void CorrectCoordinates()
        {
            if (spawnCell.x < 0)
                spawnCell.x += 10;

            if (spawnCell.x > 19)
                spawnCell.x -= 10;

            if (spawnCell.y < 0)
                spawnCell.y += 10;

            if (spawnCell.y > 19)
                spawnCell.y -= 10;
        }
        public void Activate()
        {
            StartTimer();
            StartCoroutine(DebugSpawn());
        }

        public void Deactivate()
        {
            StopAllCoroutines();
        }

        private void StartTimer()
        {
            isTimerActive = true;
        }
    }
}