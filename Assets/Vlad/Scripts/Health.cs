using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    protected int _healthPoints = 5;
    [SerializeField]
    public int _maxHealthPoint = 7;


    public UnityEvent EventOnTakeDamage;
    public UnityEvent EventOnDie;

    public virtual void TakeDamage(int damageValue)
    {
        _healthPoints -= damageValue;
        if (_healthPoints <= 0)
        {
            Die();
        }
        EventOnTakeDamage.Invoke();
    }

    public virtual void AddHealth(int healthValue)
    {
        _healthPoints = _healthPoints + healthValue > _maxHealthPoint ? _maxHealthPoint : _healthPoints + healthValue;
    }

    public virtual void Die()
    {
        EventOnDie.Invoke();
    }
}
