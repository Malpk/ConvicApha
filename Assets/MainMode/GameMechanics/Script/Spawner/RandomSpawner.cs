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

        private Vector3 spawnPosition;
        private Vector3Int spawnCell;
        private bool isTimerActive = false;
        private bool spawned = false;

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
                var device = _kiFactory.GetRandomKI();              

                if (_playGround.TryGetFreeRandomCell(_player.transform.position, _radiusSpawnZone, out spawnCell))
                {
                    if (!(spawnCell.x < 0 || spawnCell.y < 0 || spawnCell.x >= 19 || spawnCell.x >= 19))
                        device.transform.position = spawnCell;
                    else
                    {
                        CorrectCoordinates();
                        device.transform.position = spawnCell;
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