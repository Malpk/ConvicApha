using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwitchMode;

namespace Underworld
{
    public class Cleaner : GameMode, ISequence
    {
        [Header("Game Setting")]
        [SerializeField] private float _speedRotation;
        [SerializeField] private float _delayDestroy;

        private int[] _direction = new int[] { -1, 1 };

        private bool _status = true;

        public override bool statusWork => _status;

        public void Constructor(SwitchMods swictMode)
        {
            Vector3 player = swictMode.playerTransform.position;
            var angel = Vector2.Angle(transform.right, player);
            angel = CorrectAngel(angel, player);
            transform.rotation = Quaternion.Euler(Vector3.forward * angel);
            ChooseDirection();
            StartCoroutine(RunMode());
        }

        private float CorrectAngel(float angel,Vector2 player)
        {
            if (player.y < transform.position.y)
                return -angel;
            return angel;
        }
        private void ChooseDirection()
        {
            int index = Random.Range(0, _direction.Length);
            _speedRotation *= _direction[index];
        }
        protected override void ModeUpdate()
        {
            transform.rotation *= Quaternion.Euler(Vector3.forward * _speedRotation * Time.deltaTime);
        }

        private IEnumerator RunMode()
        {
            yield return new WaitForSeconds(_delayDestroy);
            _status = false;
        }


    }
}