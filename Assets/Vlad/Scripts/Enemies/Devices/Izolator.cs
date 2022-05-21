using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseMode;

public class Izolator : Device
{
    [SerializeField]
    private float _activeTime = 10f;
    [SerializeField]
    private Animator[] _animators;

    private bool _isActive;
    protected void ActivateDevice()
    {
        if (_isActive)
        {
            return;
        }
        foreach (Animator animator in _animators)
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