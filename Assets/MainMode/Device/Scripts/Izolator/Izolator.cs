using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode;

public abstract class Izolator : Device
{
    [SerializeField] protected float activeTime = 10f;
    [SerializeField] protected Transform _jetHolder;
    [SerializeField] protected ISetAttack[] _jets; 

    private bool _isActive;
    protected Animator[] animators;

    protected override void Intilizate()
    {
        _jets = GetComponentsInChildren<ISetAttack>();
        foreach (var jet in _jets)
        {
            jet.SetAttack(AttackInfo);
        }
        animators = _jetHolder.GetComponentsInChildren<Animator>();
        SetMode(false);
    }
    protected void ActivateDevice()
    {
        if (_isActive)
        {
            return;
        }
        foreach (Animator animator in animators)
        {
            animator.SetTrigger("Activate");
        }
        _isActive = true;
        Invoke(nameof(DeactivateDevice), activeTime);
        SetMode(_isActive);
    }

    private void DeactivateDevice()
    {
        foreach (Animator animator in animators)
        {
            animator.SetTrigger("Deactivate");
        }
        _isActive = false;
        SetMode(_isActive);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (attackInfo.Effect == EffectType.None)
            return;
        if (collision.TryGetComponent<PlayerScreen>(out PlayerScreen screen))
        {
            screen.SetEffect(attackInfo.Effect);
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (attackInfo.Effect == EffectType.None)
            return;
        if (collision.TryGetComponent<PlayerScreen>(out PlayerScreen screen))
        {
            screen.ScreenOff(attackInfo.Effect);
        }
    }

    protected abstract void SetMode(bool mode);
    public override void TurnOff()
    {
        base.TurnOff();
        DeactivateDevice();
    }
}