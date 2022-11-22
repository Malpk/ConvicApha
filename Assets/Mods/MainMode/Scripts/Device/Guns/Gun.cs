using UnityEngine;

namespace MainMode
{
    public abstract class Gun : DeviceV2
    {
        [Header("Reference")]
        [SerializeField] protected DamageInfo attackInfo;
        [SerializeField] protected Animator gunAnimator;

        protected Player target;

        private void OnEnable()
        {
            OnActivate += Launch;
        }
        private void OnDisable()
        {
            OnActivate -= Launch;
        }

        protected abstract void Launch();

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player target))
            {
                this.target = target;
                if (!IsActive && IsShow)
                    Activate();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player target))
            {
                this.target = null;
            }
        }
    }
}