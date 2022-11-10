using MainMode.GameInteface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MainMode.Mode1921
{
    [RequireComponent(typeof(Player))]
    public class OxyGenSet : MonoBehaviour, IPlayerComponent, IBlock, ISender
    {
        [Header("Time Setting")]
        [Min(3)]
        [SerializeField] private int _safeBaseTime = 3;
        [Header("Hit Setting")]
        [Min(1)]
        [SerializeField] private int _damage = 1;
        [Min(0)]
        [SerializeField] private float _hitDelay = 1f;
        [SerializeField] private DamageInfo _hitAttack;
        [Header("Requred Reference")]
        [SerializeField] private OxyGenDisplay _display;

        private bool _isBlock;
        private float _safeTime;
        private float _curretAirSupply;
        private Player _player;
        private Coroutine _corotine;

        public float CurretAirSupply => _curretAirSupply;

        public TypeDisplay TypeDisplay => TypeDisplay.OxyGenUI;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }
        private void Start()
        {
            Play();
        }

        public bool StartUpdate()
        {
            if (_corotine == null)
                _corotine = StartCoroutine(OxyGenFieldUpdate());
            else
                return false;
            return true;
        }
        private IEnumerator OxyGenFieldUpdate()
        {
            while (_player.IsPlay)
            {
                if (_curretAirSupply > 0)
                {
                    yield return new WaitWhile(() => _isBlock); 
                    _curretAirSupply -= Time.deltaTime;
                    _display.UpdateField(_curretAirSupply / _safeTime);
                    yield return null;
                }
                else
                {
                    yield return StartCoroutine(HitDamage());
                }
            }
            _corotine = null;
        }
        private IEnumerator HitDamage()
        {
            while (_player.IsPlay && _curretAirSupply <= 0)
            {
                var progress = 0f;
                while (progress < 1f && _curretAirSupply <= 0)
                {
                    yield return new WaitWhile(() => _isBlock);
                    progress += Time.deltaTime / _hitDelay;
                    yield return null;
                }
                if (_curretAirSupply <= 0)
                    _player.TakeDamage(_damage, _hitAttack);
            }
        }
        public void ChangeFitre(Filtre filltre)
        {
            _safeTime = filltre.FiltrationTime;
            _curretAirSupply = _safeTime;
        }
        public void ReduceFilltre(float value = 0f)
        {
            value = Mathf.Clamp01(value);
            _curretAirSupply -= _safeTime * value;
        }
        public void Play()
        {
            _safeTime = _safeBaseTime;
            _curretAirSupply = _safeTime;
            StartUpdate();
        }

        public void Block()
        {
            _isBlock = true;
        }

        public void UnBlock()
        {
            _isBlock = false;
        }

        public bool AddReceiver(Receiver receiver)
        {
            if (_display != null)
                return false;
            if (receiver is OxyGenDisplay display)
                _display = display;
            return _display;
        }
    }
}