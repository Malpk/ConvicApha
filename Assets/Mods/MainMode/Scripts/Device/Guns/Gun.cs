using UnityEngine;

namespace MainMode
{
    public abstract class Gun : DeviceV2
    {
        [Header("Reference")]
        [SerializeField] protected DamageInfo attackInfo;
        [SerializeField] protected Animator gunAnimator;
        [SerializeField] protected DeviceSpawner _spawner;

        protected Player target;
        protected Player lastTarget;

        protected override void ActivateDevice()
        {
            if(_spawner)
                _spawner.Play();
        }
        protected override void DeactivateDevice()
        {
            if (_spawner)
                _spawner.Stop();
        }
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
                lastTarget = target;
                this.target = null;
            }
        }
    }
}