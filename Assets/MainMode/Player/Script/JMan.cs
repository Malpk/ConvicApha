using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JMan : Character
{
    [Header("Ability")]
    [SerializeField] private float _timeReload;
    [SerializeField] private float _forceJerk;
    [Range(0,1f)]
    [SerializeField] private float _debafSpeed;

    private float _stoneEffect = 1;
    private float _stopEffect = 1;
    private float _abilitiSpeedEffect = 1;
    private Vector2 _direction;

    private bool _isUseEffect = true;

    private Coroutine _reload;
    private Coroutine _stopMoveCorotine;
    private Coroutine _utpdateMoveCorotine;

    public override bool IsUseEffect => _isUseEffect;

    private void FixedUpdate()
    {
        if (isDead)
            return;
        _direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Move(_direction);
        if (_direction != Vector2.zero)
            _movement.Rotate(_direction, speedRotaton);
    }
    private void Move(Vector2 direction)
    {
        var move = direction * speedMovement * _abilitiSpeedEffect * _stoneEffect * _stopEffect;
        _movement.Move(move);
        if (Input.GetAxis("Jump") != 0 && _reload == null)
            rigidBody.AddForce(Jerk(),ForceMode2D.Impulse);
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

    public override void Dead()
    {
        isDead = true;
        animator.SetTrigger("Dead");
        health.EventOnDead.Invoke();
        rigidBody.velocity = Vector2.zero;
        if (respawn == null)
            respawn = StartCoroutine(ReSpawn());
    }

    public override void TakeDamage(int damage)
    {
        if (!_isUseEffect)
            return;
        health.SetDamage(damage);
        if (health.Health <= 0)
            Dead();
        else
            health.EventOnTakeDamage.Invoke();
    }
    public override void ChangeSpeed(float duration, float value = 1)
    {
        if (_utpdateMoveCorotine == null && _isUseEffect)
            _utpdateMoveCorotine = StartCoroutine(UpdateSpeed(duration, value));
    }

    public override void StopMove(float timeStop)
    {
        if (_stopMoveCorotine == null && _isUseEffect)
            _stopMoveCorotine = StartCoroutine(StopMovement(timeStop));
    }


    private IEnumerator StopMovement(float duratuin)
    {
        _stopEffect = 0f;
        animator.SetBool("Freez", true);
        yield return new WaitForSeconds(duratuin);
        animator.SetBool("Freez", false);
        _stopEffect = 1;
        _stopMoveCorotine = null;
    }
    private IEnumerator UpdateSpeed(float duration, float value)
    {
        _stoneEffect = value;
        yield return new WaitForSeconds(duration);
        _stoneEffect = 1;
        _utpdateMoveCorotine = null;
    }
}
