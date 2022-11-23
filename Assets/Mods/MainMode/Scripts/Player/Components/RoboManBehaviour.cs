using System.Collections;
using UnityEngine;

namespace PlayerComponent
{
    public class RoboManBehaviour : PlayerBaseBehaviour
    {
        [Header("Character Setting")]
        [Min(0)]
        [SerializeField] private float _reduceSpeed;
        [Min(0)]
        [SerializeField] private float _reduceTemperatureTime;
        [Range(0, 1)]
        [SerializeField] private float _bodyTemperature = 0;
        [Range(0, 1)]
        [SerializeField] private float _temperatureSteep = 0.15f;

        [SerializeField] private SpriteRenderer _spriteRender;

        private Color _startColor;
        private Coroutine _cooling;

        private void Awake()
        {
            _startColor = _spriteRender.color;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            PlayAction += PlayRobotMan;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            PlayAction -= PlayRobotMan;
        }
        private void PlayRobotMan()
        {
            _bodyTemperature = 0;
            PlaytCooling();
            ChangeTemaerature();
            _spriteRender.color = _startColor;
        }

        public override void AddEffects(MovementEffect effect, float timeActive)
        {
            if (effect.Effect == EffectType.Freez && _bodyTemperature > 0.3f)
            {
                _bodyTemperature = 0;
                ChangeTemaerature();
            }
            else
            {
                base.AddEffects(effect, timeActive);
            }
        }
        public override bool TakeDamage(int damage, DamageInfo damgeInfo)
        {
            if (damgeInfo.Attack == AttackType.Fire)
            {
                if (ChangeTemaerature(_temperatureSteep * damage))
                    Dead();
                return true;
            }
            return base.TakeDamage(damage, damgeInfo);
        }

        private bool ChangeTemaerature(float temperature = 0)
        {
            _bodyTemperature = Mathf.Clamp01(_bodyTemperature + temperature);
            _spriteRender.color = Vector4.MoveTowards(_startColor, Color.red, _bodyTemperature);
            return _bodyTemperature >= 1f;
        }
        private void PlaytCooling()
        {
            if (_cooling == null)
            {
                _cooling = StartCoroutine(Cooling());
            }
        }
        private IEnumerator Cooling()
        {
            while (IsPlay)
            {
                yield return new WaitWhile(() => _bodyTemperature == 0);
                yield return new WaitForSeconds(_reduceTemperatureTime);
                ChangeTemaerature(-_reduceSpeed);
            }
            _cooling = null;
        }
    }
}