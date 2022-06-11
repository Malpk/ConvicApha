using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode;

public class CristalMan : Player
{
    [Header("Ability Setting")]
    [Min(1)]
    [SerializeField] private float _timeReload = 1;
    [SerializeField] private ReturnPoint _returnPoint;

    private bool _isReload = false;

    private List<AttackType> _dangersAttack = new List<AttackType>() { AttackType.Explosion, AttackType.Kinetic};
    private List<AttackType> _save = new List<AttackType>() { AttackType.Fire, AttackType.Venom };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isReload)
            ActiveAbility();
    }

    private void ActiveAbility()
    {
        if (_returnPoint.State)
        {
            transform.position = _returnPoint.Position;
            _returnPoint.Deactive(this);
            _isReload = true;
            Invoke("Reload", _timeReload);
            TakeDamage(1, null);
        }
        else
        {
            _returnPoint.ActiveMode(transform.position);
        }
    }
    public override void TakeDamage(int damage, AttackInfo type)
    {
        if (type != null)
        {
            if (_save.Contains(type.Attack))
                return;
            damage *= _dangersAttack.Contains(type.Attack) ? 2 : 1;
        }
        base.TakeDamage(damage, type);
    }
    private void Reload()
    {
        _isReload = false;
    }
}
