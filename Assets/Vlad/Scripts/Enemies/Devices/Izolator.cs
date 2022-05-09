using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Izolator : Device
{
    [SerializeField]
    private float _activeTime = 10f;
    [SerializeField]
    private Animator[] _animators;

    private bool _isActive;
    protected override void ActivateDevice(EffectsHandler effectsHandler)
    {
        if (_isActive)
        {
            return;
        }
        foreach(Animator animator in _animators)
        {
            animator.SetTrigger("Activate");
        }
        _isActive = true;
        Invoke(nameof(DeactivateDevice), _activeTime);
    }

    private void DeactivateDevice()
    {
        foreach (Animator animator in _animators)
        {
            animator.SetTrigger("Deactivate");
        }
        _isActive = false;
    }

}
