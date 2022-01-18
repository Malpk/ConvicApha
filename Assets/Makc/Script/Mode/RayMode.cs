using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class RayMode : GameMode
    {
        [Header("Game Setting")]
        [SerializeField] private int _countRay;
        [SerializeField] private float _duration;
        [SerializeField] private float _speedRotation;

        [Header("Scene Setting")]
        [SerializeField] private GameObject _ray;


        private bool _status = true;

        public override bool statusWork => _status;

        private void Start()
        {
            CreateRay();
            StartCoroutine(Rotation());
        }
        private void CreateRay()
        {
            var steepRotation = 360 / _countRay;
            var lostSteep = 0f;
            for (int i = 0; i < _countRay; i++)
            {
                var ray = Instantiate(_ray).transform;
                ray.parent = transform;
                ray.rotation = Quaternion.Euler(Vector3.forward * lostSteep);
                lostSteep += steepRotation;
            }
        }
        protected override void ModeUpdate()
        {
        }

        private IEnumerator Rotation()
        {
            var progress = 0f;
            while (progress < 1f)
            {
                transform.rotation *= Quaternion.Euler(Vector3.forward * _speedRotation*Time.deltaTime);
                Debug.Log(Vector3.forward * _speedRotation * Time.deltaTime);
                progress += Time.deltaTime / _duration;
                yield return null;
            }
            _status = false;
        }
    }
}