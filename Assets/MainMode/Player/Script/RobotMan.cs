using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode;

[RequireComponent(typeof(CapsuleCollider2D))]
public class RobotMan : Player
{
    [Header("Ability")]
    [Min(1)]
    [SerializeField] private float _abilityReloadTime = 1;
    [Min(1)]
    [SerializeField] private float _distanceAttack = 1;
    [SerializeField] private LayerMask _deviceLayer;
    [Header("Debafe Character")]
    [Min(1)]
    [SerializeField] private float _timeDebaf;
    [Range(0, 1)]
    [SerializeField] private float _debafSpeed;
    [Range(0,1)]
    [SerializeField] private float _bodyTemperature = 0;
    [Range(0, 1)]
    [SerializeField] private float _temperatureSteep = 0.15f;
    [Range(0, 1)]
    [SerializeField] private float _temperaturOnHit = 0.2f;
    [Header("Requred Perfab")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator _lightHit;

    private CapsuleCollider2D _collider;
    
    private int _countDebaf = 0;
    private float _debafSpeedCurret = 1f;
    private IMode _target = null;

    private Coroutine _abillityActive = null;

    protected override float SpeedMovement => base.SpeedMovement * _debafSpeedCurret;

    protected override void Awake()
    {
        _collider = GetComponent<CapsuleCollider2D>();
        base.Awake();
    }

    private void OnValidate()
    {
        ChangeTemaerature(_bodyTemperature);
    }


    private void Update()
    {
        TrakingDevice();
        if (Input.GetAxis("Jump") != 0 && _abillityActive==null)
        {
            ChangeTemaerature(_temperaturOnHit);
            _abillityActive = StartCoroutine(AbillitReload());
            _lightHit.SetTrigger("Hit");
            if (_target != null)
            {
                _target.TurnOff();
                _target = null;
            }
        }
    }
    private IEnumerator AbillitReload()
    {
        yield return new WaitForSeconds(_abilityReloadTime);
        _abillityActive = null;
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
            _target = hit.transform.GetComponent<IMode>();
        else
            _target = null;
    }
    public override void TakeDamage(int damage, EffectType type)
    {

        switch (type)
        {
            case EffectType.Venom:
                return;
            case EffectType.Fire:
                ChangeTemaerature(_temperatureSteep);
                break;
            default:
                base.TakeDamage(damage, type);
                return;
        }
    }
    public override void StopMove(float timeStop, EffectType effect = EffectType.None)
    {
        if (effect == EffectType.Freez)
            {
            if (_bodyTemperature < 0.3f)
            {
                base.StopMove(timeStop);
            }
            _bodyTemperature = 0;
            ChangeTemaerature();
        }
        else
            base.StopMove(timeStop);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent<JetSteam>(out JetSteam jet))
        {
            _debafSpeedCurret = _debafSpeed;
            _countDebaf++;
            Invoke("DropDebaf", _timeDebaf);
        }
    }
    private void ChangeTemaerature(float temperature = 0)
    {
        if (_bodyTemperature > 1f)
            Dead();
        _bodyTemperature += temperature;
        var value = Mathf.Clamp01(1 - (temperature == 0 ?_temperatureSteep : 0 + _bodyTemperature));
        _sprite.color = new Vector4(1,value, value, 1);
    }
    private void DropDebaf()
    {
        _countDebaf--;
        if (_countDebaf == 0)
             _debafSpeedCurret = 1;
    }
    protected override void ResetCharacter()
    {
        _bodyTemperature = 0;
        ChangeTemaerature();
        _debafSpeedCurret = 1;
        base.ResetCharacter();
    }
}
