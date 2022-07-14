using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JMan : Player
{
    [Header("Ability")]
    [SerializeField] private float _timeReload;
    [SerializeField] private float _forceJerk;
    [Range(0,1f)]
    [SerializeField] private float _debafSpeed;

    private float _abilitiSpeedEffect = 1;

    private bool _isUseEffect = true;

    private Coroutine _reload = null;

    public override bool IsUseEffect => _isUseEffect;

    protected override float SpeedMovement => base.SpeedMovement * _abilitiSpeedEffect;

    protected override void Move(Vector2 direction)
    {
       
        base.Move(direction);
    }
    protected override void UseAbillity()
    {
        rigidBody.AddForce(Jerk());
    }
    private Vector2 Jerk()
    {
        if (_reload == null)
        {
            _reload = StartCoroutine(Reload());
        }
        else
        {
            return Vector2.zero;
        }       
        _abilitiSpeedEffect = _debafSpeed;
        _isUseEffect = false;
        playerCollider.enabled = false;
        return transform.up * _forceJerk;
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(_timeReload);
        playerCollider.enabled = true; 
        _abilitiSpeedEffect = 1f;
        _isUseEffect = true;
        _reload = null;
    }

    public override void TakeDamage(int damage, DamageInfo type)
    {
        if (_isUseEffect)
            base.TakeDamage(damage, type);
    }

    public override void StopMove(float timeStop, EffectType effect)
    {
        if(_isUseEffect)
            base.StopMove(timeStop, effect);
    }
    public override void ChangeSpeed(float duration, EffectType effect, float value = 1)
    {
        if (_isUseEffect)
            base.ChangeSpeed(duration, effect, value);
    }
}
