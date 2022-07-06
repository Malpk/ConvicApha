using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MainMode.Mode1921
{
    [RequireComponent(typeof(Player))]
    public class OxyGenField : MonoBehaviour, IPlayerComponent
    {
        [Header("Time Setting")]
        [Min(3)]
        [SerializeField] private int _safeBaseTime = 3;
        [Header("Hit Setting")]
        [Min(1)]
        [SerializeField] private int _damage = 1;
        [Min(0)]
        [SerializeField] private float _hitDelay = 1f;
        [SerializeField] private AttackInfo _hitAttack;
        [Header("Requred Reference")]
        [SerializeField] private Image _fieldImage;

        private float _safeTime;
        private float _curretOxyValue;
        private Player _player;
        private Coroutine _corotine;

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
            while (!_player.IsDead)
            {
                if (_curretOxyValue > 0)
                {
                    _curretOxyValue -= Time.deltaTime;
                    _fieldImage.fillAmount = _curretOxyValue / _safeTime;
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
            while (!_player.IsDead && _curretOxyValue <= 0)
            {
                var progress = 0f;
                while (progress < 1f && _curretOxyValue <= 0)
                {
                    progress += Time.deltaTime / _hitDelay;
                    yield return null;
                }
                if (_curretOxyValue <= 0)
                    _player.TakeDamage(_damage, _hitAttack);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Filtre>(out Filtre filtre))
            {
                _safeTime = filtre.FiltrationTime;
                _curretOxyValue = _safeTime;
                filtre.Pick();
            }
        }
        public void Play()
        {
            _safeTime = _safeBaseTime;
            _curretOxyValue = _safeTime;
            StartUpdate();
        }
    }
}