using MainMode;
using MainMode.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;


[RequireComponent(typeof(PlayerScreen), typeof(Collider2D))]
public class Player : Character, IResist
{
    [SerializeField] protected Inventory _inventory;
    [SerializeField] protected Controller controller;
    [Header("Resist setting")]
    [SerializeField] protected List<AttackType> _defoutResistAttack;
    [SerializeField] protected List<EffectType> _defoutResistEffect;

    protected float stopEffect = 1;
    protected float stoneEffect = 1;
    protected Collider2D playerCollider = null;
    protected PlayerScreen screenEffect = null;
    private IItemInteractive _interacive = null;

    private Coroutine _stopMoveCorotine;
    private Coroutine _utpdateMoveCorotine;

    protected Dictionary<AttackType, int> attackResist = new Dictionary<AttackType, int>();
    protected Dictionary<EffectType, int> effectResist = new Dictionary<EffectType, int>();

    public delegate void Messange();
    public event Messange OnDead;

    public override bool IsDead => isDead;

    protected virtual float speedMovement => state.SpeedMovement * GetMovementEffect();
    protected virtual float speedRotation => state.SpeedRotation;

    protected override void Awake()
    {
        screenEffect = GetComponent<PlayerScreen>();
        playerCollider = GetComponent<Collider2D>();
        base.Awake();
    }
    #region SetController
    private void OnEnable()
    {
        if (controller != null)
            BindController(controller);
    }
    private void OnDisable()
    {
        if (controller != null)
            UnBindController(controller);
    }
    public void SetController(Controller controller)
    {
        this.controller = controller;
        BindController(controller);
    }
    protected void BindController(Controller controller)
    {
        controller.InteractiveAction += InteractiveWhithObject;
        controller.UseItemAction += UseItem;
        controller.UseArtifactAction += UseArtifact;
        controller.MovementAction += Move;
        controller.UseAbillityAction += UseAbillity;
    }
    protected void UnBindController(Controller controller)
    {
        controller.InteractiveAction -= InteractiveWhithObject;
        controller.UseItemAction -= UseItem;
        controller.UseArtifactAction -= UseArtifact;
        controller.MovementAction -= Move;
        controller.UseAbillityAction += UseAbillity;
    }
    #endregion
    protected override void Move(Vector2 direction)
    {
        if (isDead)
            return;
        var speedDebaf = GetMovementEffect();
        movement.Move(direction * speedMovement);
        if (direction != Vector2.zero)
            _baseMovement.Rotate(direction, speedRotation * speedDebaf);
    }
    #region Player Damage and Dead
    public override void Dead()
    {
        isDead = true;
        if (OnDead != null)
            OnDead();
        animator.SetBool("Dead", true);
        rigidBody.velocity = Vector2.zero;
        if (respawn == null && isAutoRespawnMode)
            respawn = StartCoroutine(ReSpawn());
    }
    public override void TakeDamage(int damage, DamageInfo damageInfo)
    {
        if (!IsResist(damageInfo.Attack))
        {
            health.SetDamage(damage);
            if (health.Health <= 0)
                Dead();
            screenEffect.ShowEffect(damageInfo);
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
    #region Interactive and Useble
    private void InteractiveWhithObject()
    {
        if (_interacive != null)
        {
            _interacive.Interactive(this);
        }
    }
    private void UseItem()
    {
        if (_inventory.TryGetConsumableItem(out Item item))
        {
            item.Use();
        }
    }
    private void UseArtifact()
    {
        if (_inventory.TryGetArtifact(out Artifact artifact))
        {
            artifact.Use();
        }
    }
    public void ApplyEffect(IItemEffect itemEffect)
    {
        itemEffect.UseEffect(this);
    }
    protected virtual void UseAbillity()
    {

    }
    #endregion
    public void Heal(int point)
    {
        health.Heal(point);
    }
    public override void Respawn()
    {
        stopEffect = 1f;
        stoneEffect = 1f;
        base.Respawn();
    }

    public void AddDefaultItems(ConsumablesItem consumablesItem, Artifact artifact)
    {
        if (consumablesItem != null && artifact != null)
        {
            _inventory.AddConsumablesItem(consumablesItem);
            _inventory.AddArtifact(artifact);
#if UNITY_EDITOR 
            Debug.Log($"default items was added!");
#endif
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogError($"default items have not been added!");
        }
#endif
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
        if (collision.TryGetComponent(out IItemInteractive interactive))
        {
            _interacive = interactive;
        }
        if (collision.TryGetComponent(out VenomCloud cloud))
        {
            screenEffect.ShowEffect(cloud.DamageInfo.Effect);
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IItemInteractive interactive))
        {
            _interacive = null;
        }
        if (collision.TryGetComponent(out VenomCloud cloud))
        {
            TakeDamage(0, cloud.DamageInfo);
            screenEffect.ScreenHide(cloud.DamageInfo.Effect);
        }
    }
    #region SetResist
    public void AddResistAttack(DamageInfo attackInfo, float timeActive)
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
