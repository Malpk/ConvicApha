using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public sealed class Term : SmartItem
    {
        [Header("Reference")]
        [SerializeField] private Fire _termFire;
        [SerializeField] private Animator _termAnimator;
        [SerializeField] private SpriteRenderer _termSprite;

        private bool _isDamageMode;

        private bool _isActive = false;


        private IDamage _target;
        private DeviceStateWork _previus;

        public bool IsActive => _isActive;
        public bool IsDamageMode => _isDamageMode;

        private DeviceStateWork State { get; set; }

        private void Awake()
        {
            HideTerm();
        }
        private void OnEnable()
        {
            ShowItemAction += ShowTerm;
            HideItemAction += HideTerm;
        }
        private void OnDisable()
        {
            ShowItemAction -= ShowTerm;
            HideItemAction -= HideTerm;
        }
        #region Object Display 
        private void ShowTerm()
        {
            DisplayTerm(true);
        }
        private void HideTerm()
        {
            DisplayTerm(false);
        }
        #endregion
        #region Object Work

        public void Activate(FireState firestate = FireState.Start)
        {
            if (!_isActive)
            {
                _isActive = true;
                SetMode(true);
                State = DeviceStateWork.Play;
                _termFire.Activate(firestate);
                if (_target != null)
                    _target.Explosion();
            }
        }

        public void Deactivate(bool waitAnimation = true)
        {
            if (_isActive)
            {
                _isActive = false;
                StartCoroutine(StartDeactivate(waitAnimation));
            }
        }

        public IEnumerator HideByDeactivation()
        {
#if UNITY_EDITOR
            if (_isActive)
                throw new System.Exception("The term is not currently in the process of deactivation");
#endif
            yield return new WaitWhile(() => _isDamageMode && !_isActive);
            if (!_isActive)
            {
                if(IsShow)
                    HideItem();
            }
        }

        #endregion
        private IEnumerator StartDeactivate(bool waitAnimation)
        {
            if (waitAnimation)
            {
                _termFire.DeactivateWaitAnimation();
                yield return new WaitWhile(() => _termFire.CurretState != FireState.End && !_isActive);
            }
            else
            {
                _termFire.DeactiveEvent();
            }
            if (!_isActive)
            {
                SetMode(false);
                State = DeviceStateWork.Stop;
            }
        }

        private void DisplayTerm(bool mode)
        {
            _termSprite.enabled = mode;
        }

        private void SetMode(bool mode)
        {
            _isDamageMode = mode;
            _termAnimator.SetBool("Activate", mode);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamage target))
            {
                _target = target;
                if (_isDamageMode)
                    _target.Explosion();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamage target))
            {
                _target = null;
            }
        }

    }
}
