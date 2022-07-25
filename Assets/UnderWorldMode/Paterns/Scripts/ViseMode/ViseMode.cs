using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Underworld
{
    public class ViseMode : MonoBehaviour
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
                gameObject.SetActive(false);
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
