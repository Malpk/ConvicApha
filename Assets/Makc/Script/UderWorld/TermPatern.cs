using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;

namespace Underworld
{
    public class TermPatern : TernBase
    {
        [Header("Game Setting")]
        [SerializeField] private bool _inverMode;
        [SerializeField] private float _delay;

        [Header("Pefab Setting")]
        [SerializeField] private GameObject _fire;

        private GameObject _fireInstiate;
        private TernState _state = TernState.Warning;
        private TernState _lostState;
        private Animator _animator;


        private void Start()
        {
            if (_inverMode)
                TurnOff();
            StartCoroutine(Work());
        }

        protected override IEnumerator Work()
        {
            yield return new WaitForSeconds(_delay);
            _fireInstiate = InstatiateFire(_fire);
            _animator = _fireInstiate.GetComponent<Animator>();
            ChangeState();
            if (_state == TernState.Deactive)
                SetState(false);
            yield return new WaitWhile(() => (_fireInstiate != null));
            Destroy(gameObject);
        }

        private void ChangeState()
        {
            if (_state == TernState.Warning)
                _state = TernState.Fire;
            else
                _lostState = TernState.Fire;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<GameMode>() != null)
            {
                if (!_inverMode)
                    TurnOff();
                else
                    TurnOn();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<GameMode>() != null)
            {
                if (!_inverMode)
                    TurnOn();
                else
                    TurnOff();
            }
        }
        private void TurnOn()
        {
            _state = _lostState;
            SetState(true);
        }
        public void TurnOn(TernState state)
        {
            _state = state;
            SetState(true);
        }
        public void TurnOff()
        {
            _lostState = _state;
            _state = TernState.Deactive;
            SetState(false);
        }
        private void SetState(bool value)
        {
            if (_fireInstiate != null)
            {
                _fireInstiate.GetComponent<SpriteRenderer>().enabled = value;
            }
            GetComponent<SpriteRenderer>().enabled = value;
        }
        public void SetTurnMode(bool mode)
        {
            if (!mode && _animator != null && _state == TernState.Fire)
                _animator.SetTrigger("Fading");
            else
                Destroy(gameObject);
        }

        protected override void Damage(Player player)
        {
            if (_state == TernState.Fire)
                player.Term();
        }
    }
}
