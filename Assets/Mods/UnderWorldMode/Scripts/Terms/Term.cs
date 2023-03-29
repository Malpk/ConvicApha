using UnityEngine;

namespace Underworld
{
    public sealed class Term : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private Fire _termFire;
        [SerializeField] private Animator _termAnimator;
        [SerializeField] private GameObject _termBody;

        private bool _isActive;
        private IDamage _target;

        public bool IsShow { get; private set; } = false;
        public bool IsActive => _isActive;

        private void Awake()
        {
            Hide();
        }

        private void OnEnable()
        {
            _termFire.OnDeactivateFire += () => SetMode(false);
        }
        private void OnDisable()
        {
            _termFire.OnDeactivateFire -= () => SetMode(false);
        }
        public void Activate(FireState firestate = FireState.Start)
        {
#if UNITY_EDITOR
            if (!IsShow)
                throw new System.Exception("you con't activate hide term");
#endif
            if (!_isActive)
            {
                _isActive = true;
                SetMode(true);
                _termFire.Activate(firestate);
                if (_target != null)
                    _target.Explosion(AttackType.Fire);
            }
        }

        public void Deactivate(bool waitAnimation = true)
        {
            if (waitAnimation)
            {
                _termFire.DeactivateWaitAnimation();
            }
            else
            {
                SetMode(false);
                _termFire.Deactivete();
            }
        }
        public void Show()
        {
            IsShow = true;
            _termBody.SetActive(true);
        }
        public void Hide()
        {
#if UNITY_EDITOR
            if (_isActive)
                throw new System.NullReferenceException("попытка спрятать активный объект");
#endif
            IsShow = false;
            _termBody.SetActive(false);
        }

        private void SetMode(bool mode)
        {
            _isActive = mode;
            _termAnimator.SetBool("Activate", mode);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player target))
            {
                _target = target;
                if (_isActive)
                    _target.Explosion(AttackType.Fire);
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

