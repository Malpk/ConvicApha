using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{

    private float _stoneEffect = 1;
    private float _stopEffect = 1;

    private Coroutine _stopMoveCorotine;
    private Coroutine _utpdateMoveCorotine;

    public override bool IsUseEffect => true;

    protected override void Awake()
    {
        base.Awake();
    }

    protected void FixedUpdate()
    {
        if (isDead)
            return;
        var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _movement.Move(direction * speedMovement * _stoneEffect * _stopEffect);
        if (direction != Vector2.zero)
            _movement.Rotate(direction,speedRotaton);
    }
    public override void Dead()
    {
        isDead = true;
        animator.SetTrigger("Dead");
        health.EventOnDead.Invoke();
        rigidBody.velocity = Vector2.zero;
        if(respawn == null)
            respawn = StartCoroutine(ReSpawn());
    }
    public override void TakeDamage(int damage)
    {
        health.SetDamage(damage);
        if (health.Health <= 0)
        {
            Dead();
        }
        else
        {
            health.EventOnTakeDamage.Invoke();
        }
    }
    public override void StopMove(float timeStop)
    {
        if (_stopMoveCorotine == null)
            _stopMoveCorotine = StartCoroutine(StopMovement(timeStop));
    }
    public override void ChangeSpeed(float duration, float value = 1)
    {
        if (_utpdateMoveCorotine == null)
            _utpdateMoveCorotine = StartCoroutine(UpdateSpeed(duration, value));
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
