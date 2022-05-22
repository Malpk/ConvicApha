using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseMode;

public abstract class Izolator : MonoBehaviour
{
    [SerializeField] protected float activeTime = 10f;
    [SerializeField] protected Transform _jetHolder;

    private bool _isActive;
    protected Animator[] animators;
    public abstract EffectType TypeEffect { get; }

    protected virtual void Awake()
    {
        animators = _jetHolder.GetComponentsInChildren<Animator>();
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
    }

    private void DeactivateDevice()
    {
        foreach (Animator animator in animators)
        {
            animator.SetTrigger("Deactivate");
        }
        _isActive = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (TypeEffect == EffectType.None)
            return;
        if (collision.TryGetComponent<PlayerEffect>(out PlayerEffect screen))
        {
            screen.SetEffect(TypeEffect);
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (TypeEffect == EffectType.None)
            return;
        if (collision.TryGetComponent<PlayerEffect>(out PlayerEffect screen))
        {
            screen.ScreenOff(TypeEffect);
        }
    }
}