using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class CoruselMode : GameMode, IModeForSwitch
    {
        [Header("Movement Setting")]
        [SerializeField] private float _speedRotation;
        [Header("Time Setting")]
        [Min(0)]
        [SerializeField] private float _duration;
        [Min(0)]
        [SerializeField] private float _warningTime;

        private int[] _direction = new int[] { -1, 1 };
        private Point[,] _map;

        public bool IsActive => startMode != null;

        public void Constructor(SwitchMode swictMode)
        {
            if (startMode != null)
                return;
            _map = swictMode.builder.Map;
            ActivateTile(_map);
            transform.rotation = Quaternion.Euler(Vector3.forward * 
                DefineStartAngel(swictMode.Player.Position));
            startMode = StartCoroutine(RunMode());
        }

        public void SetSetting(string jsonSetting)
        {
            var setting = JsonUtility.FromJson<CoruselSetting>(jsonSetting);
            _duration = setting.Duration;
            _warningTime = setting.WarningTime;
            _speedRotation = setting.SpeedRotation;
        }

        private int ChooseDirection()
        {
            int index = Random.Range(0, _direction.Length);
            return _direction[index];
        }
        private float DefineStartAngel(Vector2 player)
        {
            var angel = Vector2.Angle(transform.right, player);
            if (player.y < transform.position.y)
                return -angel;
            return angel;
        }
        private IEnumerator RunMode()
        {
            yield return new WaitForSeconds(_warningTime);
            float progress = 0f;
            _speedRotation += ChooseDirection();
            yield return new WaitForSeconds(_warningTime);
            while (progress <= 1)
            {
                progress += Time.deltaTime / _duration;
                transform.rotation *= Quaternion.Euler(Vector3.forward * _speedRotation * Time.deltaTime);
                yield return null;
            }
            yield return new WaitWhile(() => TurnOffPoints(_map).IsActive);
            startMode = null;
            gameObject.SetActive(false);
        }
    }
}