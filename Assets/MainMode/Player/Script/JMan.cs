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
    [SerializeField] private LayerMask _wallLayer;

    private Coroutine _reload = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        UseAbillityAction += JerkForward;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UseAbillityAction -= JerkForward;
    }
    private void JerkForward()
    {
        if (_reload == null)
        {
            AddEffects(_debaf, _timeActiveDebaf);
            var distance = _jerkDistance;
            var hit = Physics2D.Raycast(transform.position, transform.up, distance, _wallLayer);
            if (hit)
            {
                distance = Vector2.Distance(transform.position, hit.point);
            }
            _reload = StartCoroutine(Jerk(distance));
        }
    }

    private IEnumerator Jerk(float distance)
    {
        var progress = 0f;
        var startPosition = transform.position;
        var direction = transform.up;
        playerCollider.enabled = false;
        while (progress < 1f)
        {
            rigidBody.MovePosition(startPosition + direction * progress * distance);
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
