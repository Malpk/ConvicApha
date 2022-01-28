using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using SwitchMode;
using UnityEngine.Tilemaps;
namespace Underworld
{
    public class ViseMode : MonoBehaviour, ISequence
    {
        [Header("Game Setting")]
        [SerializeField] private int _lenght;
        [SerializeField] private float _fireTime;
        [SerializeField] private float _warnigTime;
        [SerializeField] private bool _dubleMode = false;

        [Header("Perfab Setting")]
        [SerializeField] private GameObject _vise;

        Vector3 _offset;
        private int[] _direction = new int[] { 1, -1 };
        private Vector3[] _angls = new Vector3[] { Vector3.right, Vector3.up };

        public void Constructor(SwitchMods swictMode)
        {
            _offset = swictMode.tileMap.cellSize;
            StartCoroutine(CreateVise());
        }

        private IEnumerator CreateVise()
        {
            GameObject lostVise = null;
            if (_dubleMode)
            {
                foreach (var direction in _direction)
                {
                    foreach (var angle in _angls)
                    {
                        lostVise = InstateViseLine(angle * direction);
                    }
                }
            }
            else
            {
                var index = Random.Range(0, _angls.Length);
                foreach (var direction in _direction)
                {
                    lostVise = InstateViseLine(_angls[index] * direction);
                }
            }
            yield return new WaitWhile(() => lostVise != null);
            Destroy(gameObject);
        }
        private GameObject InstateViseLine(Vector3 direction)
        {
            var viseInstate = Instantiate(_vise, transform.position, Quaternion.identity);
            viseInstate.transform.parent = transform;
            if (viseInstate.TryGetComponent<Vise>(out Vise vise))
            {
                vise.Constructor(_lenght, _offset.x, direction, _fireTime, _warnigTime);
            }
            return viseInstate;
        }
    }
}