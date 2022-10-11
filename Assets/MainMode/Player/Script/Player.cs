using MainMode;
using MainMode.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;


[RequireComponent(typeof(PlayerScreen), typeof(Collider2D))]
public class Player : Character, IResist
{
    [SerializeField] protected MarkerUI _markerUI;
    [SerializeField] protected Inventory _inventory;
    [SerializeField] protected Controller controller;
    [Header("Resist setting")]
    [SerializeField] protected List<AttackType> _defoutResistAttack;
    [SerializeField] protected List<EffectType> _defoutResistEffect;

    protected CapsuleCollider2D playerCollider = null;
    protected PlayerScreen screenEffect = null;

    private IItemInteractive _interacive = null;

    protected Dictionary<AttackType, int> attackResist = new Dictionary<AttackType, int>();
    protected Dictionary<EffectType, int> effectResist = new Dictionary<EffectType, int>();

    public event System.Action DeadAction;

    protected event System.Action UseAbillityAction;

    protected virtual float speedMovement => state.SpeedMovement * GetMovementEffect();
    protected virtual float speedRotation => state.SpeedRotation;

    protected override void Awake()
    {
        screenEffect = GetComponent<PlayerScreen>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        base.Awake();
    }
    #region Intilizate
    protected virtual void OnEnable()
    {
        if (controller != null)
            BindController(controller);
    }
    protected virtual void OnDisable()
    {
        if (controller != null)
            UnBindController(controller);
    }
    public void SetMarker(MarkerUI marker)
    {
        _markerUI = marker;
    }
    public void SetControoler(Controller controller)
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
        if (IsPlay)
        {
            var speedDebaf = GetMovementEffect();
            movement.Move(direction * speedMovement);
            if (direction != Vector2.zero)
                _baseMovement.Rotate(direction, speedRotation * speedDebaf);
        }
    }
    #region Player Damage and Dead
    public override void Explosion()
    {
        if (IsPlay)
        {
            Stop();
            animator.SetBool("Dead", true);
            rigidBody.velocity = Vector2.zero;
            if (respawn == null && isAutoRespawnMode)
                respawn = StartCoroutine(ReSpawn());
        }
    }
    public override void TakeDamage(int damage, DamageInfo damageInfo)
    {
        if (!IsResist(damageInfo.Attack))
        {
            health.SetDamage(damage);
            if (health.Health <= 0)
                Explosion();
            screenEffect.ShowEffect(damageInfo);
        }
    }
    private void DeadAnimationEvent()
    {
        if (DeadAction != null)
            DeadAction();
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
        if (_inventory.TryGetConsumableItem(out ConsumablesItem item))
        {
            item.Use();
        }
    }
    private void UseArtifact()
    {
        if (_inventory.TryGetArtifact(out Item artifact))
        {
            if (artifact.UseType == ItemUseType.Shoot)
            {
                _markerUI.ShowMarker();
                transform.rotation = Quaternion.Euler(Vector3.forward * (_markerUI.CurretAngel-90));
            }
            artifact.Use();
        }
    }
    private void UseAbillity()
    {
        if (UseAbillityAction != null)
            UseAbillityAction();
    }
    #endregion
    public void Heal(int point)
    {
        health.Heal(point);
    }


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IPickable itemUse))
        {
            if (itemUse is ConsumablesItem consumablesItem)
            {
                consumablesItem.Pick(this);
                _inventory.AddConsumablesItem(consumablesItem);
            }
            else if (itemUse is Item item)
            {
                item.Pick(this);
                _inventory.AddArtifact(itemUse as Item);
            }
        }
        if (collision.TryGetComponent(out IItemInteractive interactive))
        {
            _interacive = interactive;
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IItemInteractive interactive))
        {
            _interacive = null;
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
