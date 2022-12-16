using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using UnityEngine.Events;

public class PlayerBehaviour : MonoBehaviour, IResist
{
    [SerializeField] private string _name;
    [Min(1)]
    [SerializeField] private int _maxHealth = 1;
    [SerializeField] private PlayerResistContainer _playerResist;
    [Header("Reference")]
    [SerializeField] private PlayerAnimator _animator;

    private int _health;
    protected List<IPlayerTask> tasks = new List<IPlayerTask>();

    public UnityEvent OnDead;
    public UnityEvent<int> OnSetupMaxHealth;
    public UnityEvent<int> OnUpdateHealth;

    public int Health => _health;
    protected bool IsPlay { get; private set; } = false;

    public void Start()
    {
        _health = _maxHealth;
        OnSetupMaxHealth.Invoke(_maxHealth);
        tasks.Add(_playerResist);
    }
    public virtual void Play()
    {
        IsPlay = true;
        Heal(_maxHealth);
        _animator.ResetAnimator();
    }

    public virtual void Stop()
    {
        IsPlay = false;
        _playerResist.Reset();
    }
    public void UpdateBehaviour()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].Update();
        }
    }
    public void AddResistAttack(DamageInfo damage, float timeActive)
    {
        _playerResist.Add(damage.Effect, damage, timeActive);
    }
    public bool Heal(int healValue)
    {
        if (_health < _maxHealth)
        {
            _health = Mathf.Clamp(_health + healValue, _health, _maxHealth);
            OnUpdateHealth.Invoke(_health);
            return true;
        }
        return false;
    }
    public virtual bool TakeDamage(int damage, DamageInfo damageInfo)
    {
        if (!_playerResist.ContainResistAttack(damageInfo.Attack))
        {
            _health = Mathf.Clamp(_health - damage, 0, _health);
            OnUpdateHealth.Invoke(_health);
            if (_health == 0)
                Explosion();
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Explosion()
    {
        if (IsPlay)
        {
            IsPlay = false;
            _animator.SetAnimation(PlayerState.Dead, true);
        }
    }

    public void SetAnimation(PlayerState state, bool mode)
    {
        _animator.SetAnimation(state, mode);
    }

    public void DeadAnimationEvent()
    {
        OnDead.Invoke();
    }
    public void Invulnerability(bool mode)
    {
        SetAnimation(PlayerState.Invulnerability, mode);
    }
    public void ResetState()
    {
        _health = _maxHealth;
        OnUpdateHealth.Invoke(_health);
    }
}
