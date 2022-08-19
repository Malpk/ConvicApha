using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Underworld
{
    public class CoruselMode : TotalMapMode
    {
        [Header("Movement Setting")]
        [SerializeField] private float _speedRotation;
        [Min(0)]
        [SerializeField] private float _warningTime;

        private Coroutine _runMode;
        private int[] _direction = new int[] { -1, 1 };

        public override bool Activate()
        {
            if (_runMode == null && IsReady)
            {
                State = ModeState.Play;
                _runMode = StartCoroutine(RunMode());
                return true;
            }
            return false;
        }
        private IEnumerator RunMode()
        {
            var speed = _speedRotation;
            var task = Task.Run(() =>
            {
                transform.rotation *= Quaternion.Euler(Vector3.forward * 
                    DefineStartAngel(player.transform.position) * Time.deltaTime);
                speed *= ChooseDirection();
            });
            yield return WaitTime(_warningTime);
            yield return new WaitWhile(()=> task.IsCompleted);
            float progress = 0f;
            while (progress <= 1 && State != ModeState.Stop)
            {
                yield return new WaitWhile(() => State == ModeState.Pause);
                progress += Time.deltaTime / workDuration;
                transform.rotation *= Quaternion.Euler(Vector3.forward * speed * Time.deltaTime);
                yield return null;
            }
            DeactivateMap(out HandTermTile term);
            yield return new WaitWhile(() => term.IsActive);
            State = ModeState.Stop;
            _runMode = null;
        }
        private float DefineStartAngel(Vector2 player)
        {
            var angel = Vector2.Angle(transform.right, player);
            if (player.y < transform.position.y)
                return -angel;
            return angel;
        }
        private int ChooseDirection()
        {
            int index = Random.Range(0, _direction.Length);
            return _direction[index];
        }
    }
}