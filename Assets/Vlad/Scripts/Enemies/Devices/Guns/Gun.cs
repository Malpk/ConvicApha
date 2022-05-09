using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Gun : MonoBehaviour
{
    [SerializeField] protected GunsEnum gunType = GunsEnum.Gun;
    public GunsEnum GunType => gunType;

    [Header("Attacks properties")]
    [SerializeField]
    protected float _activeTime = 6f;
    [SerializeField]
    protected float _activationTime = 1f;
    [SerializeField]
    protected float _firingRateOnSeconds = 1f;
    [SerializeField]
    protected GameObject _bulletPrefab;
    [SerializeField]
    protected Transform _spawnTransform;
    [Header("Move properties")]
    [SerializeField]
    protected float _rotationAngleOnSeconds;
    [SerializeField]
    protected Animator _animator;

    protected Rigidbody2D _rigidbody;
    protected bool _isActive;
    protected EffectsHandler _effectsHandler;
    private float _time;

    protected void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    
    protected virtual IEnumerator Rotate()
    {
        _time = _firingRateOnSeconds;
        float startAngle = _rigidbody.rotation;
        for (float f = 0; f < _activeTime; f += Time.deltaTime)
        {
            _rigidbody.rotation = Mathf.Lerp(_rigidbody.rotation, _rigidbody.rotation + _rotationAngleOnSeconds, Time.deltaTime);
            _time += Time.deltaTime;
            Shoot();
            yield return null;
        }
        _rigidbody.rotation = startAngle;
        _isActive = false;
    }
    protected virtual void Shoot()
    {
        if(_time >= _firingRateOnSeconds)
        {
            _time = 0;
            _animator.SetTrigger("Shoot");
            Bullet bullet = Instantiate(_bulletPrefab, _spawnTransform.position, _spawnTransform.rotation).GetComponent<Bullet>();
        }
    }

    public virtual void ActivateGun(EffectsHandler effectsHandler)
    {
        if(_isActive)
        {
            return;
        }
        _isActive = true;
        _effectsHandler = effectsHandler;
        StartCoroutine(Rotate());
    }
}
