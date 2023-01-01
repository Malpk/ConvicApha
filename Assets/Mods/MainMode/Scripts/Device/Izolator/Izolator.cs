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
        private Player _object;

        public override bool IsCompliteWork => IsComplite();

        protected void Awake()
        {
            foreach (var jet in jets)
            {
                jet.SetAttack(attackInfo);
                jet.Deactivate(false);
            }
        }

        private void Update()
        {
            _progress += Time.deltaTime / _activateTime;
            if (_progress >= 1f && _object == null)
                Deactivate();
        }

        protected override void ActivateDevice()
        {
            _progress = 0f;
            foreach (var jet in jets)
            {
                jet.Activate();
            }
        }
        protected override void DeactivateDevice()
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                if(IsShow)
                    Activate();
                _object = player;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                _object = null;
                if(IsShow)
                    Activate();
            }
        }
    }
}