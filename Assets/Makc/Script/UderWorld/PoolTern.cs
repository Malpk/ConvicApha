using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [RequireComponent(typeof(Animator))]
    public class PoolTern : TernBase, ITileAnimation
    {
        [Header("Game Setting")]
        [SerializeField] private GameObject _fire;

        private Animator _tileAnimator;
        private Animator _fireAnimator;
        private GameObject _instateFire;
        private Coroutine _offTile = null;

        public override TernState state => _instateFire != null ? TernState.Fire : TernState.Deactive;

        protected override void Damage(Player player)
        {
            if (state == TernState.Fire)
                player.Incineration();
        }

        protected override void Intializate()
        {
            _tileAnimator = GetComponent<Animator>();
        }

        protected override IEnumerator Work()
        {
            yield return null;
        }
        public bool StartTile()
        {
            soundSource.Play();
            _tileAnimator.SetInteger("State", 1);
            if (_instateFire == null)
            {
                _instateFire = InstatiateFire(_fire);
                _fireAnimator = _instateFire.GetComponent<Animator>();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Stop()
        {
            if (!gameObject.activeSelf)
            {
                Destroy(_instateFire);
                return true;
            }
            if (_offTile == null)
            {
                _offTile = StartCoroutine(TurnOffTile());
                return true;
            }
            else
            {
                soundSource.Stop();
                return false;
            }
        }
        private IEnumerator TurnOffTile()
        {
            if (_fireAnimator != null)
                _fireAnimator.SetTrigger("Fading");
            else
                Destroy(_instateFire);
            yield return new WaitWhile(() => _instateFire != null);
            _tileAnimator.SetInteger("State", 0);
            soundSource.Stop();
            gameObject.SetActive(false);
        }

        public bool IdleMode()
        {
            if (_fireAnimator != null)
                _fireAnimator.SetBool("IdleMode", true);
            return _fireAnimator;
        }
    }
}