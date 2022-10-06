using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Effects;

public class JMan : Player
{
    [Header("Ability")]
    [Min(1)]
    [SerializeField] private float _timeReload;
    [SerializeField] private float _durationJerk;
    [SerializeField] private float _timeActiveDebaf;
    [SerializeField] private float _jerkDistance;
    [SerializeField] private MovementEffect _debaf;

    private Coroutine _reload = null;

    protected override void UseAbillity()
    {
        if (_reload == null)
        {
            AddEffects(_debaf, _timeActiveDebaf);
            _reload = StartCoroutine(Jerk());
        }
    }

    private IEnumerator Jerk()
    {
        var progress = 0f;
        var startPosition = transform.position;
        var direction = transform.up;
        playerCollider.enabled = false;
        while (progress < 1f)
        {
            rigidBody.MovePosition(startPosition + direction * progress * _jerkDistance);
            progress += Time.deltaTime / _durationJerk;
            yield return null;
        }
        playerCollider.enabled = true;
        yield return new WaitForSeconds(_timeReload);
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
