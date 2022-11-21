using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Izolator : DeviceV2
    {
        [SerializeField] private float _activateTime;
        [Header("Reference")]
        [SerializeField] protected JetPoint[] jets;
        [SerializeField] protected DamageInfo attackInfo;

        private float _progress = 0f;

        public override bool IsCompliteWork => IsComplite();

        protected void Awake()
        {
            foreach (var jet in jets)
            {
                jet.SetAttack(attackInfo);
                jet.Deactivate(false);
            }
        }
        protected virtual void OnEnable()
        {
            OnActivate += ActivateIzolator;
            OnDeactivate += DeactivateIzolator;
        }
        protected virtual void OnDisable()
        {
            OnActivate -= ActivateIzolator;
            OnDeactivate -= DeactivateIzolator;
        }
        private void Update()
        {
            _progress += Time.deltaTime / _activateTime;
            if (_progress >= 1f)
                Deactivate();
        }

        public void ActivateIzolator()
        {
            _progress = 0f;
            foreach (var jet in jets)
            {
                jet.Activate();
            }
        }
        public void DeactivateIzolator()
        {
            foreach (var jet in jets)
            {
                jet.Deactivate();
            }
        }

        private bool IsComplite()
        {
            foreach (var jet in jets)
            {
                if (jet.IsActive)
                    return false;
            }
            return true;
        }
    }
}