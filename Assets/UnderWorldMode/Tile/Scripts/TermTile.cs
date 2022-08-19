using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public abstract class TermTile : SpawnItem,IPause
    {
        [Header("Reference")]
        [SerializeField] protected Fire fire;
        [SerializeField] protected Animator tileAnimator;

        [SerializeField] private SpriteRenderer _spriteTile;

        protected bool isPause = false;
        protected bool isDangerMode = false;

        private bool _isShow = true;
        private BoxCollider2D _bocCollider;

        private Coroutine _deactivate;
        private Coroutine _fireDeactive;

        public override bool IsShow => _isShow;
        public virtual bool IsActive => fire.CurretState != FireState.End;

        private void Awake()
        {
            _bocCollider = GetComponent<BoxCollider2D>();
            Intilizate();
        }

        private void Intilizate()
        {
            _bocCollider.isTrigger = true;
        }

        #region  Tile Work
        public void Deactivate(bool waitCompliteFire = true)
        {
            if (_fireDeactive == null)
            {
                if (fire.CurretState != FireState.End)
                    OffFire(waitCompliteFire);
                _fireDeactive = StartCoroutine(FireDeactivate());
            }
        }
        private IEnumerator FireDeactivate()
        {
             yield return new WaitWhile(() => fire.CurretState != FireState.End);
            isDangerMode = false;
            tileAnimator.SetBool("Activate", false);
            _fireDeactive = null;
        }
        private void OffFire(bool waitAnimation)
        {
            if (waitAnimation)
                fire.DeactivateWaitAnimation();
            else
                fire.Deactive();
        }
        #endregion
        #region Tile Mode
        public override void SetMode(bool mode)
        {
            if (mode && !_isShow)
            {
                SetState(mode);
            }
            else
            {
                OffItem();
            }
        }

        private void SetState(bool mode)
        {
            _isShow = mode;
            _bocCollider.enabled = mode;
            _spriteTile.enabled = mode;
        }
        public override void OffItem()
        {
            if (_deactivate == null)
            {
                if (fire.CurretState != FireState.End)
                    OffFire(true);
                _deactivate = StartCoroutine(OffTile(fire));
            }
        }
        private IEnumerator OffTile(Fire fire)
        {
            yield return new WaitWhile(() => fire.CurretState != FireState.End);
            SetState(false);
            _deactivate = null;
        }
        #endregion
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isDangerMode)
                return;
            if (collision.TryGetComponent(out IDamage target))
            {
                target.Dead();
            }
        }
        #region Pause Mode
        public void Pause()
        {
            isPause = true;
        }

        public void UnPause()
        {
            isPause = false;
        }
        #endregion
    }
}