using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour
{
    [SerializeField]
    protected float _multiplierSpeed = 5f;
    [SerializeField]
    private float _maxMultiplierSpeed = 10f;
    [SerializeField]
    protected float _multiplierSpeedRotation = 10f;

    protected float _currentSpeed;

    protected virtual void Awake()
    {
        _currentSpeed = _multiplierSpeed;
    }

    public void SetMultiplierSpeed(float value)
    {
        _currentSpeed = Mathf.Clamp(_multiplierSpeed * value, 0, _maxMultiplierSpeed);
    }
}
