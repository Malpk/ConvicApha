using UnityEngine;

namespace MainMode
{
    public abstract class JetPoint : MonoBehaviour, ISetAttack
    {
        [Header("Reference")]
        [SerializeField] private Animator _jetAnimator;

        public bool IsActive { get; private set; }

        public abstract void SetAttack(DamageInfo info);

        public void Activate()
        {
            SetState(true);
            _jetAnimator.SetBool("Mode", true); 
        }
        public void Deactivate(bool waitAnimation = true)
        {
            if (waitAnimation)
                _jetAnimator.SetBool("Mode", false);
            else
                SetState(false);
        }

        private void SetDeactivateStateAnimatuinEvent()
        {
            SetState(false);
        }
        private void SetState(bool mode)
        {
            IsActive = mode;
            gameObject.SetActive(mode);
        }
    }
}