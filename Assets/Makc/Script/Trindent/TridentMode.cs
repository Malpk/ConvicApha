using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using SwitchMode;

namespace Trident 
{
    public class TridentMode : BaseTridentMode, ISequence
    {
        [Header("Game Setting")]
        [SerializeField] private int _offset;
        [SerializeField] private float _warningTime;
        [SerializeField] private float _duration;
        [SerializeField] private float _delay;

        private Tilemap _tileMap;
        private Coroutine _coroutine;

        public bool IsAttackMode => true;

        public void Constructor(SwitchMods swictMode)
        {
            if (_coroutine != null)
                return;
            _tileMap = swictMode.tileMap;
            Intializate(_tileMap);
            _coroutine = StartCoroutine(RunMode());
        }

        protected override IEnumerator RunMode()
        {
            float progress = 0;
            GameObject lostTrident = null;
            while (progress <= 1f)
            {
                CreateTrindent(_offset, _warningTime);
                yield return new WaitForSeconds(_delay);
                progress += _delay / _duration;
            }
            yield return new WaitWhile(() => lostTrident != null);
            Destroy(gameObject);
        }
    }
}