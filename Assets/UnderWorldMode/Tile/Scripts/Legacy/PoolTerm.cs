using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class PoolTerm : DetecDevice, ITileMode
    {
        [Header("Requred Setting")]
        [SerializeField] private Fire _fire;

        private bool _mode = true;
        private Animator _tileAnimator;
        private Coroutine _offTile = null;
        private SpriteRenderer _spriteBody;

        public bool IsActive => _fire.CurretState != FireState.End;
        public override TileState state => _fire != null ? TileState.Dangerous : TileState.Safe;

        protected override void Intializate()
        {
            _tileAnimator = GetComponent<Animator>();
            _spriteBody = GetComponent<SpriteRenderer>();
        }
        #region Set mode
        public bool Activate(FireState state = FireState.Start, TileInfo timeActive = null)
        {
            if (_offTile != null)
                return false;
            _mode = true;
            _offTile = StartCoroutine(TurnOn(timeActive, state));
            return true;
        }
        public void Deactivate()
        {
            if (_offTile != null)
                _offTile = StartCoroutine(Deactive());
        }
        public void SetMode(bool mode)
        {
            _mode = mode;
            _spriteBody.enabled = mode;
        }
        public IEnumerator WarningMode()
        {
            _fire.Diactivate();
            yield return new WaitWhile(() => _fire != null);
            SetState(TileState.Safe);
        }
        #endregion
        #region Tile Deactive

        private IEnumerator TurnOn(TileInfo timeDeactive,FireState state)
        {
            yield return Activate(state, timeDeactive != null ? timeDeactive.WarningTime : 0);
            yield return Deactive(timeDeactive != null ? timeDeactive.ActivateTine : 0);
        }
        private IEnumerator Activate(FireState state, float warning = 0)
        {
            yield return new WaitForSeconds(warning);
            SetState(TileState.Dangerous);
            _fire.Activate(state);
            yield return new WaitWhile(() => IsActive && _mode);
            SetState(TileState.Safe);
            SetMode(false);
            _offTile = null;
        }
        private IEnumerator Deactive(float timeDeactive = 0)
        {
            var progress = 0f;
            while (progress <= 1f && _mode)
            {
                progress += Time.deltaTime / timeDeactive;
                yield return null;
            }
            _fire.Diactivate();
        }
        #endregion
        #region Set state
        private void SetState(TileState state)
        {
            _tileAnimator.SetInteger("State", GetState(state));
        }
        private int GetState(TileState state)
        {
            switch (state)
            {
                case TileState.Dangerous:
                    return 1;
                default:
                    return 0;
            }
        }
        #endregion
        protected override void Damage(IDamage player)
        {
            player.Dead();
        }


    }
}