using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode;
using PlayerComponent;

public class CristalMan : Player
{
    [Header("Ability Setting")]
    [Min(1)]
    [SerializeField] private float _timeReload = 1;
    [SerializeField] private ReturnPoint _returnPoint;
    [SerializeField] private DamageInfo _returnDamage;

    private bool _isReload = false;

    private List<AttackType> _dangersAttack = new List<AttackType>() { AttackType.Explosion, AttackType.Kinetic};

    protected override void UseAbillity()
    {
        if (_isReload)
            return;
        if (_returnPoint.State)
        {
            transform.position = _returnPoint.Position;
            _returnPoint.Deactive(this);
            _isReload = true;
            Invoke("Reload", _timeReload);
            TakeDamage(1, _returnDamage);
        }
        else
        {
            _returnPoint.ActiveMode(transform.position);
        }
    }
    public override void TakeDamage(int damage, DamageInfo type)
    {
        if (IsResist(type.Attack))
            return;
        damage *= _dangersAttack.Contains(type.Attack) ? 2 : 1;
        base.TakeDamage(damage, type);
    }
    private void Reload()
    {
        _isReload = false;
    }
}
