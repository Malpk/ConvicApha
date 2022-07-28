using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Effects;

public class JMan : Player
{
    [Header("Ability")]
    [Min(1)]
    [SerializeField] private float _timeActive;
    [SerializeField] private float _forceJerk;
    [Range(0,1f)]
    [SerializeField] private MovementEffect _debaf;

    private Coroutine _reload = null;

    protected override void UseAbillity()
    {
        rigidBody.AddForce(Jerk());
    }
    private Vector2 Jerk()
    {
        if (_reload == null)
        {
            AddEffects(_debaf, _timeActive);
            _reload = StartCoroutine(Reload());
        }
        else
        {
            return Vector2.zero;
        }
        IsUseEffect = false;
        playerCollider.enabled = false;
        return transform.up * _forceJerk;
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(_timeActive);
        playerCollider.enabled = true;
        IsUseEffect = true;
        _reload = null;
    }

    public override void TakeDamage(int damage, DamageInfo type)
    {
        if (IsUseEffect)
            base.TakeDamage(damage, type);
    }
}
