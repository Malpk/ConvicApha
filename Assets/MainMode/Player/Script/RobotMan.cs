using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode;
using MainMode.Effects;

[RequireComponent(typeof(CapsuleCollider2D))]
public class RobotMan : Player
{
    [Header("Ability")]
    [Min(1)]
    [SerializeField] private float _abilityReloadTime = 1;
    [Min(1)]
    [SerializeField] private float _distanceAttack = 1;
    [SerializeField] private LayerMask _deviceLayer;
    [Header("Character Setting")]
    [Min(0)]
    [SerializeField] private float _reduceSpeed;
    [Min(0)]
    [SerializeField] private float _reduceTemperatureTime;
    [Range(0, 1f)]
    [SerializeField] private float _movementDebaf = 0.7f;
    [Range(0, 1f)]
    [SerializeField] private float _rotationDebaf = 0.7f;
    [Range(0, 1)]
    [SerializeField] private float _bodyTemperature = 0;
    [Range(0, 1)]
    [SerializeField] private float _temperatureSteep = 0.15f;
    [Range(0, 1)]
    [SerializeField] private float _temperaturOnHit = 0.2f;
    [Header("Requred Perfab")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator _lightHit;

    protected override float speedMovement => base.speedMovement * _movementDebaf;
    protected override float speedRotation => base.speedRotation * _rotationDebaf;

    private CapsuleCollider2D _collider;
    
    private DeviceV2 _target = null;

    private Color _startColor;
    private Coroutine _abillityActive = null;
    private Coroutine _cooling;

    protected override void Awake()
    {
        _startColor = _sprite.color;
        ChangeTemaerature();
        _collider = GetComponent<CapsuleCollider2D>();
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PlayAction += PlaytCooling;
        PlayAction += PlayRobotMan;
        UseAbillityAction += ShootLighting;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayAction -= PlaytCooling;
        PlayAction -= PlayRobotMan;
        UseAbillityAction -= ShootLighting;
    }
    private void PlayRobotMan()
    {
        _bodyTemperature = 0;
        ChangeTemaerature();
        _sprite.color = _startColor;
    }

    protected void ShootLighting()
    {
        TrakingDevice();
        if (_abillityActive == null)
        {
            ChangeTemaerature(_temperaturOnHit);
            _abillityActive = StartCoroutine(AbillitReload());
            _lightHit.SetTrigger("Hit");
            if (_target != null)
            {
                if (_target.IsActive)
                    _target.Deactivate();
                if(_target.IsShow)
                    _target.HideItem();
                _target = null;
            }
        }
    }
    private void PlaytCooling()
    {
        if (_cooling == null)
        {
            _cooling = StartCoroutine(Cooling());
        }
    }
    private IEnumerator AbillitReload()
    {
        yield return new WaitForSeconds(_abilityReloadTime);
        _abillityActive = null;
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
    private void TrakingDevice()
    {
        RaycastHit2D hit = new RaycastHit2D();
        var offset = transform.right * _collider.size.x / 2;
        var cheakPosition = new Vector3[] { transform.position - offset, transform.position, transform.position + offset };
        for (int i = 0; i < cheakPosition.Length && !hit; i++)
        {
            hit = Physics2D.Raycast(cheakPosition[i], transform.up, _distanceAttack, _deviceLayer);
#if UNITY_EDITOR
            Debug.DrawLine(cheakPosition[i], cheakPosition[i] + transform.up * _distanceAttack, Color.green);
#endif
        }
        if (hit)
        {
            _target = hit.transform.GetComponent<DeviceV2>();
        }
        else
        {
            _target = null;
        }
    }
    public override void TakeDamage(int damage, DamageInfo attack)
    {
        if (attack.Effect == EffectType.Fire && !IsResist(EffectType.Fire))
        {
            ChangeTemaerature(_temperatureSteep);
        }
        base.TakeDamage(damage, attack);
    }
    public override void AddEffects(MovementEffect effect, float duration)
    {
        if (effect.Effect == EffectType.Freez)
        {
            if (_bodyTemperature < 0.3f)
            {
                base.AddEffects(effect, duration);
            }
            _bodyTemperature = 0;
            ChangeTemaerature();
        }
        else
        {
            base.AddEffects(effect, duration);
        }
    }

    private void ChangeTemaerature(float temperature = 0)
    {
        if (_bodyTemperature >= 1f)
            Explosion();
        _bodyTemperature = Mathf.Clamp01(_bodyTemperature + temperature);
        _sprite.color = Vector4.MoveTowards(_startColor, Color.red, _bodyTemperature);
    }
}
