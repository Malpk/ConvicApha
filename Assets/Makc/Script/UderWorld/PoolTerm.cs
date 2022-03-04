using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class PoolTerm : TernBase, ITileAnimation
    {
        [Header("Game Setting")]
        [SerializeField] private GameObject _fire;

        private Animator _tileAnimator;
        private Animator _fireAnimator;
        private GameObject _instateFire;
        private Coroutine _offTile = null;
        private SpriteRenderer _spriteBody;

        public bool IsActive => _spriteBody.enabled;
        public override TernState state => _instateFire != null ? TernState.Fire : TernState.Deactive;

        protected override void Damage(Player player)
        {
            if (state == TernState.Fire)
                player.Incineration();
        }

        protected override void Intializate()
        {
            _tileAnimator = GetComponent<Animator>();
            _spriteBody = GetComponent<SpriteRenderer>();
        }

        protected override IEnumerator Work()
        {
            yield return null;
        }
        public void SetActiveMode(bool mode)
        {
            _spriteBody.enabled = mode;
            _tileAnimator.enabled = mode;
            if (!mode)
            {
                Destroy(_instateFire);
            }
        }
        public bool StartTile()
        {
            if (_instateFire == null)
            {
                _tileAnimator.SetInteger("state", 1);
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
            if (!_spriteBody.enabled)
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
            _tileAnimator.SetInteger("state", 0);
            SetActiveMode(false);
            _offTile = null;
        }

        public bool IdleMode()
        {
            if (_fireAnimator != null)
                _fireAnimator.SetBool("IdleMode", true);
            return _fireAnimator;
        }
    }
}