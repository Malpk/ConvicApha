using SwitchModeComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Trident
{
    public class TriggerTridentMode : BaseTridentMode, ISequence
    {
        [Header("Game Setting")]
        [SerializeField] private int _offset;
        [SerializeField] private float _warningTime;
        [SerializeField] private float _delay;

        private bool _state = false;
        private Coroutine _coroutine;
        private Tilemap _tileMap;

        public bool IsAttackMode => true;

        public void Constructor(SwitchMods swictMode)
        {
            _tileMap = swictMode.tileMap;
            Intializate(_tileMap);
        }
        public bool State => _coroutine != null ? true : false;
        
        public bool ActiveteMode()
        {
            if (_coroutine == null)
            {
                _state = true;
                _coroutine = StartCoroutine(RunMode());
                return true;
            }
            else
            {
                return false;
            }
        }
        public void DeactivateMode()
        {
            Debug.Log(_state);
            _state = false;
        }
        protected override IEnumerator RunMode()
        {
            while (_state)
            {
                CreateTrindent(_offset, _warningTime);
                yield return new WaitForSeconds(_delay);
            }
            _coroutine = null;
        }
    }
}