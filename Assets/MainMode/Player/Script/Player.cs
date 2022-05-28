using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    protected float stopEffect = 1;
    protected float stoneEffect = 1;

    protected virtual float SpeedMovement => speedMovement * stopEffect * stoneEffect;

    private Coroutine _stopMoveCorotine;
    private Coroutine _utpdateMoveCorotine;

    public override bool IsUseEffect => true;

    protected void FixedUpdate()
    {
        var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Move(direction);
    }
    public override void Dead()
    {
        isDead = true;
        animator.SetBool("Dead",true);
        health.EventOnDead.Invoke();
        rigidBody.velocity = Vector2.zero;
        if(respawn == null)
            respawn = StartCoroutine(ReSpawn());
    }
    public override void TakeDamage(int damage, EffectType type )
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
    public override void StopMove(float timeStop, EffectType effect = EffectType.None)
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
        stopEffect = 0f;
        animator.SetBool("Freez", true);
        yield return new WaitForSeconds(duratuin);
        animator.SetBool("Freez", false);
        stopEffect = 1;
        _stopMoveCorotine = null;
    }
    private IEnumerator UpdateSpeed(float duration, float value)
    {
        stoneEffect = value;
        yield return new WaitForSeconds(duration);
        stoneEffect = 1;
        _utpdateMoveCorotine = null;
    }

    protected override void Move(Vector2 direction)
    {
        if (isDead)
            return;
        _movement.Move(direction * SpeedMovement);
        if (direction != Vector2.zero)
            _movement.Rotate(direction, speedRotaton);
    }
    protected override void ResetCharacter()
    {
        stopEffect = 1f;
        stoneEffect = 1f;
        base.ResetCharacter();
    }
}
