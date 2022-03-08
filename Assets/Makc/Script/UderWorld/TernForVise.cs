using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [RequireComponent(typeof(Animator),typeof(AudioSource))]
    public class TernForVise : TernBase
    {
        [Header("Perfab Setting")]
        [SerializeField] private GameObject _fire;

        private GameObject _instateFire;
        private Animator _tileAnimator;
        private Animator _fireAniamtor;
        private Coroutine _turnOff = null;

        public override TernState state => _instateFire == null ? TernState.Deactive : TernState.Fire;

        protected override void Intializate()
        {
            _tileAnimator = GetComponent<Animator>();
        }

        protected override void Damage(Player player)
        {
            if (state == TernState.Fire)
                player.DeadIncineration();
        }

        public void DeactiveFire()
        {
            FireFadingAnimation();
            StartCoroutine(WaitDestroyFire());
        }
        private void FireFadingAnimation()
        {
            if (_fireAniamtor != null)
            {
                _fireAniamtor.SetTrigger("turnOff");
            }
            else if (_instateFire = null)
            {
                Destroy(_instateFire);
            }
        }    
        private IEnumerator WaitDestroyFire()
        {
            yield return new WaitWhile(() => _instateFire != null);
            _tileAnimator.SetInteger("State", 0);
        }
        public void TurnOffTile()
        {
            if (_turnOff == null)
                _turnOff = StartCoroutine(TurnOff());
        }
        public void ActiveFire()
        {
            if(_instateFire == null)
                 StartCoroutine(Work());
        }
        private IEnumerator TurnOff()
        {
            FireFadingAnimation();
            yield return new WaitWhile(() => _instateFire != null);
            Destroy(gameObject);
            _turnOff = null;
        }
        protected override IEnumerator Work()
        {
            _tileAnimator.SetInteger("State", 1);
            _instateFire = InstatiateFire(_fire);
            _fireAniamtor = _instateFire.GetComponent<Animator>();
            yield return null;
        }
    }
}