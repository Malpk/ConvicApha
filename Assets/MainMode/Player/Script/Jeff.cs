using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jeff : Player
{
    [Header("Heal Ability")]
    [SerializeField] private int _healValue;
    [SerializeField] private float _delayAbillity;

    private Coroutine _reserHealth;
    protected override void Start()
    {
        base.Start();
        _reserHealth = StartCoroutine(ResertUnitHealth(_delayAbillity));
    }

    private IEnumerator ResertUnitHealth(float delay)
    {
        while (!isDead)
        {
            yield return new WaitWhile(() => health.Health == health.MaxHealth);
            yield return new WaitForSeconds(delay);
            animator.SetTrigger("Heal");
            Heal(_healValue);
        }
    }
    protected override void ResetCharacter()
    {
        if (_reserHealth == null)
            _reserHealth = StartCoroutine(ResertUnitHealth(_delayAbillity));
        base.ResetCharacter();
    }
}
