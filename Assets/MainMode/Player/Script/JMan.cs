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
    private Vector2 _direction;

    private bool _isUseEffect = true;

    private Coroutine _reload;
    private Coroutine _stopMoveCorotine;
    private Coroutine _utpdateMoveCorotine;

    public override bool IsUseEffect => _isUseEffect;

    protected override void Move(Vector2 direction)
    {
        Jerk();
        base.Move(direction);
    }

    private Vector2 Jerk()
    {
        if (_reload == null)
            _reload = StartCoroutine(Reload());
        else
            return Vector2.zero;
        _abilitiSpeedEffect = _debafSpeed;
        _isUseEffect = false;
        return transform.up * _forceJerk;
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(_timeReload);
        _abilitiSpeedEffect = 1f;
        _isUseEffect = true;
        _reload = null;
    }

    public override void TakeDamage(int damage, EffectType type)
    {
        if (!_isUseEffect)
            return;
        base.TakeDamage(damage, type);
    }



}
