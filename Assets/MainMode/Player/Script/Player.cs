using MainMode;
using MainMode.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerScreen))]
public class Player : Character, IResist
{
    [SerializeField] protected Inventory _inventory;
    [SerializeField] protected Joystick _joystick;
    [Header("Resist setting")]
    [SerializeField] protected List<AttackType> _defoutResistAttack;
    [SerializeField] protected List<EffectType> _defoutResistEffect;

    protected float stopEffect = 1;
    protected float stoneEffect = 1;
    protected PlayerScreen _screenEffect;

    private Coroutine _stopMoveCorotine;
    private Coroutine _utpdateMoveCorotine;

    protected Dictionary<AttackType, int> attackResist = new Dictionary<AttackType, int>();
    protected Dictionary<EffectType, int> effectResist = new Dictionary<EffectType, int>();

    public override bool IsUseEffect => true;
    public override bool IsDead => isDead;
    protected virtual float SpeedMovement => speedMovement * stopEffect * stoneEffect;
    protected override void Awake()
    {
        _screenEffect = GetComponent<PlayerScreen>();
        base.Awake();
        _joystick = FindObjectOfType<FloatingJoystick>();

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            _joystick.gameObject.SetActive(false);

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _joystick.gameObject.SetActive(true);

        }

    }

    protected virtual void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_inventory.TryGetConsumableItem(out Item item))
                {
                    UseItem(item);
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (_inventory.TryGetArtifact(out Artifact artifact))
                {
                    UseItem(artifact);
                }
            }
        }
    }
    protected void FixedUpdate()
    {
        Vector2 direction = Vector2.zero;
        if (Application.platform == RuntimePlatform.Android)
            direction = _joystick.Direction;
        else
            direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        Move(direction);
    }
    protected override void Move(Vector2 direction)
    {
        if (isDead)
            return;
        _movement.Move(direction * SpeedMovement);
        if (direction != Vector2.zero)
            _movement.Rotate(direction, speedRotaton);
    }
    #region Player Damage and Dead
    public override void Dead()
    {
        isDead = true;
        animator.SetBool("Dead", true);
        health.EventOnDead.Invoke();
        rigidBody.velocity = Vector2.zero;
        if (respawn == null)
            respawn = StartCoroutine(ReSpawn());
    }
    public override void TakeDamage(int damage, AttackInfo attackInfo)
    {
        if (!IsResist(attackInfo.Attack))
        {
            health.SetDamage(damage);
            if (health.Health <= 0)
            {
                Dead();
            }
            else
            {
                health.EventOnTakeDamage.Invoke();
            }
            _screenEffect.ShowEffect(attackInfo);
        }
    }
    #endregion
    #region Movement and Other Debaf
    public override void StopMove(float timeStop, EffectType effect)
    {
        if (_stopMoveCorotine == null && !IsResist(effect))
        {
            _stopMoveCorotine = StartCoroutine(StopMovement(timeStop));
        }
    }
    public override void ChangeSpeed(float duration, EffectType effect, float value = 1)
    {
        if (_utpdateMoveCorotine == null && !IsResist(effect))
        {
            _utpdateMoveCorotine = StartCoroutine(UpdateSpeed(duration, value));
        }
    }

    private IEnumerator StopMovement(float duratuin)
    {
        stopEffect = 0f;
        animator.SetBool("Freez", true);
        yield return new WaitForSeconds(duratuin);
        animator.SetBool("Freez", false);
        stopEffect = 1;
        _stopMoveCorotine = null;
    }
    private IEnumerator UpdateSpeed(float duration, float value)
    {
        stoneEffect = value;
        yield return new WaitForSeconds(duration);
        stoneEffect = 1;
        _utpdateMoveCorotine = null;
    }
    #endregion
    #region Item Useble
    private void UseItem(Item item)
    {
        item.Use();
    }

    public void ApplyEffect(IItemEffect itemEffect)
    {
        itemEffect.UseEffect(this);
    }
    #endregion
    public void Heal(int point)
    {
        health.Heal(point);
    }
    protected override void ResetCharacter()
    {
        stopEffect = 1f;
        stoneEffect = 1f;
        base.ResetCharacter();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IPickable item))
        {
            if (item is ConsumablesItem)
            {
                _inventory.AddConsumablesItem(item as ConsumablesItem);
            }
            if (item is Artifact)
            {
                _inventory.AddArtifact(item as Artifact);
            }
        }
    }
    #region SetResist
    public void AddResistAttack(AttackInfo attackInfo, float timeActive)
    {
        if (!_defoutResistAttack.Contains(attackInfo.Attack))
        {
            AddAttackResist(attackInfo.Attack);
            StartCoroutine(DeleteResist(attackInfo.Attack, timeActive));
        }
        if (!_defoutResistEffect.Contains(attackInfo.Effect))
        {
            AddEffectResist(attackInfo.Effect);
            StartCoroutine(DeleteResist(attackInfo.Effect, timeActive));
        }
    }
    private void AddEffectResist(EffectType effectType)
    {
        if (effectResist.ContainsKey(effectType))
        {
            effectResist[effectType]++;
        }
        else
        {
            effectResist.Add(effectType, 1);
        }
    }
    private void AddAttackResist(AttackType effectType)
    {
        if (attackResist.ContainsKey(effectType))
            attackResist[effectType]++;
        else
            attackResist.Add(effectType, 1);
    }
    private IEnumerator DeleteResist(AttackType type, float duration)
    {
        yield return new WaitForSeconds(duration);
        attackResist[type]--;
        if (attackResist[type] <= 0)
            attackResist.Remove(type);
    }
    private IEnumerator DeleteResist(EffectType type, float duration)
    {
        yield return new WaitForSeconds(duration);
        effectResist[type]--;
        if (effectResist[type] <= 0)
            effectResist.Remove(type);
    }
    public bool IsResist(EffectType effect)
    {
        return effectResist.ContainsKey(effect) || _defoutResistEffect.Contains(effect);
    }
    public bool IsResist(AttackType attack)
    {
        return attackResist.ContainsKey(attack) || _defoutResistAttack.Contains(attack);
    }
    #endregion
}
