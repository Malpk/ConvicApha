using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using SwitchModeComponent;
using UnityEngine.Tilemaps;
namespace Underworld
{
    public class ViseMode : MonoBehaviour, IModeForSwitch
    {
        [Header("Game Setting")]
        [SerializeField] private int _lenght;
        [SerializeField] private Vector2 _fireTime;
        [SerializeField] private Vector2 _warnigTime;
        [SerializeField] private bool _dubleMode = false;

        [Header("Perfab Setting")]
        [SerializeField] private GameObject _vise;

        Vector3 _offset;
        private int[] _direction = new int[] { 1, -1 };
        private Vector3[] _angls = new Vector3[] { Vector3.right, Vector3.up };

        public bool IsAttackMode => true;

        public void Constructor(SwitchMode swictMode)
        {
            _offset = swictMode.tileMap.cellSize;
            StartCoroutine(CreateVise());
        }

        private IEnumerator CreateVise()
        {
            var viseList = new List<GameObject>();
            if (_dubleMode)
            {
                foreach (var direction in _direction)
                {
                    foreach (var angle in _angls)
                    {
                        viseList.Add(InstateViseLine(angle * direction));
                    }
                }
            }
            else
            {
                var index = Random.Range(0, _angls.Length);
                foreach (var direction in _direction)
                {
                    viseList.Add(InstateViseLine(_angls[index] * direction));
                }
            }
            yield return StartCoroutine(CheakList(viseList));
            Destroy(gameObject);
        }
        private IEnumerator CheakList(List<GameObject> list)
        {
            while (list.Count > 0)
            {
                var cheakList = new List<GameObject>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] != null)
                        cheakList.Add(list[i]);
                }
                list = cheakList;
                yield return new WaitForSeconds(0.1f);
            }
        }
        private GameObject InstateViseLine(Vector3 direction)
        {
            var viseInstate = Instantiate(_vise, transform.position, Quaternion.identity);
            viseInstate.transform.parent = transform;
            if (viseInstate.TryGetComponent<Vise>(out Vise vise))
            {
                var fireTime = Random.Range(_fireTime.x, _fireTime.y);
                var warningTime = Random.Range(_warnigTime.x, _warnigTime.y);
                vise.Constructor(_lenght, _offset.x, direction, fireTime, warningTime);
            }
            return viseInstate;
        }
    }
}