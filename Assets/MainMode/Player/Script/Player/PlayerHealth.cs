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
    [SerializeField] private HealthUI _healthUI;

    public UnityEvent EventOnTakeDamage;
    public UnityEvent EventOnDead;


    public int MaxHealth => _maxHealthPoint;

    public int Health 
    {
        get => _healthPoints;
    }

    public void Start()
    {
        if (_healthUI == null)
            return;
        _healthUI.Setup(_maxHealthPoint);
        _healthUI.DisplayHealth(_healthPoints);
    }
    public void SetDamage(int damage)
    {
        _healthPoints -= damage;
        UpdateScreen();
    }
    public void Heal(int value)
    {
        Debug.Log("Heal");
        _healthPoints += _healthPoints + value > _maxHealthPoint ? _maxHealthPoint : value;
        UpdateScreen();
    }
    private void UpdateScreen()
    {
        if (_healthUI != null)
            _healthUI.DisplayHealth(_healthPoints);
    }
}
