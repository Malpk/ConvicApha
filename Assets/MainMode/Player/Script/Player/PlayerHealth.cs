using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MainMode;

[System.Serializable]
public class PlayerHealth
{
    [SerializeField] private int _healthPoints = 5;
    [SerializeField] private int _maxHealthPoint = 7;
    [SerializeField] private HealthUI _display;

    public int MaxHealth => _maxHealthPoint;

    public int Health => _healthPoints;

    public void Start()
    {
        _display.SetupHelth(_healthPoints);
    }
    public bool SetReceiver(HealthUI display)
    {
        if (_display != null)
            return false;
        _display = display;
        return true;
    }

    public void SetDamage(int damage)
    {
        _healthPoints -= damage;
        _display.Display(_healthPoints);
    }
    public void Heal(int value)
    {
        var newHealthPoints = _healthPoints + value;
        if (newHealthPoints > _maxHealthPoint)
            _healthPoints = _maxHealthPoint;
        else
            _healthPoints = newHealthPoints;
        _display.Display(_healthPoints);
    }

}
