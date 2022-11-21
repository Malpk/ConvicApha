using UnityEngine;

namespace MainMode
{
    public abstract class Gun : DeviceV2
    {
        [Header("Reference")]
        [SerializeField] protected DamageInfo attackInfo;
        [SerializeField] protected Animator gunAnimator;

        private void OnEnable()
        {
            OnActivate += Launch;
        }
        private void OnDisable()
        {
            OnActivate -= Launch;
        }

        protected abstract void Launch();
    }
}