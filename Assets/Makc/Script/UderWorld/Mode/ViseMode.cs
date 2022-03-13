using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwitchModeComponent;
using System.Linq;

namespace Underworld
{
    public class ViseMode : MonoBehaviour,IModeForSwitch
    {
        [Header("Mods Setting")]
        [SerializeField] private float _minTimeOffset;
        [SerializeField] private float _maxTimeOffset;
        [Header("Time setting")]
        [SerializeField] private float _warningTime;
        [Header("Requrid Perfab")]
        [SerializeField] private GameObject _poolTern;

        private List<Coroutine> _runMods = new List<Coroutine>();
        private int[] _angls = new int[] { 0, 90 };
        private Vise[] _viseHolders = null;

        public bool IsAttackMode => _runMods.Count > 0;

        public void Constructor(SwitchMode swictMode)
        {
            if (_runMods.Count >  0!)
                return;
            if (_viseHolders == null)
            {
                _viseHolders = new Vise[2];
                for (int i = 0; i < _angls.Length; i++)
                {
                    var holder = GetHolder("ViseHOlder");
                    _viseHolders[i] = holder.gameObject.AddComponent<Vise>();
                    _viseHolders[i].CreateVise(_poolTern, swictMode.builder.Map, _angls[i]);
                }
            }
            foreach (var vise in _viseHolders)
            {
                var timeOffset = Random.Range(_minTimeOffset, _maxTimeOffset);
                vise.StartMove(timeOffset,swictMode.UnitSize.x);
                 _runMods.Add(StartCoroutine(MoveVise(vise, timeOffset)));
            }
        }
        private IEnumerator MoveVise(Vise vise,float timeOffset)
        {
            while (vise.IsMoveVise)
            {
                if (vise.IsMoveVise)
                {
                    yield return StartCoroutine(vise.SetIdleMode());
                }
                yield return new WaitForSeconds(_warningTime);
                if (vise.IsMoveVise)
                {
                    vise.SetFireMode();
                }
                yield return new WaitForSeconds(timeOffset * 2);
            }
            _runMods.Remove(_runMods[0]);
            if (_runMods.Count == 0)
                Destroy(gameObject);
            yield return null;
        }
        private Transform GetHolder(string name)
        {
            var holder = new GameObject(name).transform;
            holder.parent = transform;
            return holder;
        }
    }
}
