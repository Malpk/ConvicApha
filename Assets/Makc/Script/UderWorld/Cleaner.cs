using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwitchModeComponent;

namespace Underworld
{
    public class Cleaner : GameMode, ISequence
    {
        [Header("Game Setting")]
        [SerializeField] private float _speedRotation;
        [SerializeField] private float _duration;

        [Header("Perfab Setting")]
        [SerializeField] private TrisMode _trisMode;
        private int[] _direction = new int[] { -1, 1 };
        private bool _status = true;

        public override bool statusWork => _status;

        public bool IsAttackMode => true;

        public void Constructor(SwitchMods swictMode)
        {
            Vector3 player = swictMode.playerTransform.position;
            var angel = Vector2.Angle(transform.right, player);
            angel = CorrectAngel(angel, player);
            transform.rotation = Quaternion.Euler(Vector3.forward * angel);
            ChooseDirection();
            StartCoroutine(RunMode());
        }
        private void ChooseDirection()
        {
            int index = Random.Range(0, _direction.Length);
            _speedRotation *= _direction[index];
        }
        private float CorrectAngel(float angel,Vector2 player)
        {
            if (player.y < transform.position.y)
                return -angel;
            return angel;
        }
        private IEnumerator RunMode()
        {
            float progress = 0f;
            while (progress <= 1)
            {
                progress += Time.deltaTime / _duration;
                transform.rotation *= Quaternion.Euler(Vector3.forward * _speedRotation * Time.deltaTime);
                yield return null;
            }
            _status = false;
        }
    }
}