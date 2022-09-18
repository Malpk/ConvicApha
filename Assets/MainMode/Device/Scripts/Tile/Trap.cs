using System.Collections;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Trap : DeviceV2
    {
        [SerializeField] protected LayerMask playerLayer;
        [SerializeField] protected DamageInfo attackInfo;
        [SerializeField] protected SpriteRenderer _body;

        private Collider2D _collider;
        protected bool isActiveDevice;

        public override bool IsActive => isActiveDevice;

        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<Collider2D>();
            SetMode(false);
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            CompliteShowAnimation += Activate;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            CompliteShowAnimation -= Activate;
        }
        private void Start()
        {
            if(showOnStart)
                ShowItem();
            if (destroyMode)
                StartCoroutine(HideITem(durationWork));
        }
        private IEnumerator HideITem(float timeActive)
        {
            yield return new WaitForSeconds(timeActive);
            Deactivate();
            HideItem();
        }
        public override void Activate()
        {
#if UNITY_EDITOR
            if (isActiveDevice)
                throw new System.Exception("Izolator is already active");
            else if (!IsShow)
                throw new System.Exception("you can't activate a Izolator that is hide");
#endif
            isActiveDevice = true;
        }
        public override void Deactivate()
        {
#if UNITY_EDITOR
            if (!isActiveDevice)
                throw new System.Exception("Tile is already deactive");
#endif
            isActiveDevice = false;
        }

        protected override void ShowDeviceAnimationEvent()
        {
            SetMode(true);
        }
        protected override void HideDeviceAnimationEvent()
        {
            SetMode(false);
        }
        protected void SetMode(bool mode)
        {
            _body.enabled = mode;
            _collider.enabled = mode;
        }

        protected void SetScreen(Collider2D collision, DamageInfo attack)
        {
            if (collision.TryGetComponent<PlayerScreen>(out PlayerScreen screen))
            {
                screen.ShowEffect(attackInfo);
            }
        }
    }
}