using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public sealed class Term : SmartItem,IPause
    {
        [Header("Reference")]
        [SerializeField] private Fire _termFire;
        [SerializeField] private Animator _termAnimator;
        [SerializeField] private SpriteRenderer _termSprite;

        private bool _isDamageMode;

        private bool _isPause = false;
        private bool _isActive = false;

        private Coroutine _taskDeactivate;

        private IDamage _target;
        private DeviceStateWork _previus;

        public bool IsActive => _isActive;
        public bool IsDamageMode => _isDamageMode;

        private DeviceStateWork State { get; set; }

        private void Awake()
        {
            SetMode(false);
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
#if UNITY_EDITOR
            CheakHideItem();
#endif
            DisplayTerm(false);
        }
        #endregion
        #region Object Work

        public void Activate(FireState firestate = FireState.Start)
        {
#if UNITY_EDITOR
            CheakOnActive();
#endif
            _isActive = true;
            SetMode(true);
            State = DeviceStateWork.Play;
            _termFire.Activate(firestate);
            if (_target != null)
                _target.Dead();
        }

        public void Deactivate(bool waitAnimation = true)
        {
#if UNITY_EDITOR
            if (!_isActive)
                throw new System.Exception("Term is already Deactivation");
#endif
            _isActive = false;
            StartCoroutine(StartDeactivate(waitAnimation));
        }

        public void Pause()
        {
#if UNITY_EDITOR
            if (State == DeviceStateWork.Pause)
                throw new System.Exception("Term is already pause");
#endif
            _previus = State;
            State = DeviceStateWork.Pause;
        }
        public void UnPause()
        {
#if UNITY_EDITOR
            if (State != DeviceStateWork.Pause)
                throw new System.Exception("Term is not on pause");
#endif
            State = _previus;
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
                HideItem();
            }
        }

        #endregion
        private IEnumerator WaitTime(float waitTime)
        {
            var progress = 0f;
            while (progress <= 1f)
            {
                yield return new WaitWhile(() => _isPause);
                yield return null;
                progress += Time.deltaTime / waitTime;
            }
        }
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
                    _target.Dead();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamage target))
            {
                _target = null;
            }
        }

#if UNITY_EDITOR
        private void CheakOnActive()
        {
             if (_isActive)
                throw new System.Exception("Term is already activation");
        }
        private void CheakHideItem()
        {
            if (_taskDeactivate != null)
                throw new System.Exception("Deañtication has already started");
            else if (IsDamageMode)
                throw new System.Exception("You cannot hide an activated object");
        }
#endif
    }
}
