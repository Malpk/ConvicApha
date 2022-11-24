using System.Collections.Generic;
using MainMode.GameInteface;
using UnityEngine;
using System;

namespace PlayerComponent
{
    public class PlayerBaseBehaviour : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerHealth _health;
        [SerializeField] private PlayerResistContainer _playerResist;

        [SerializeField] protected PlayerAbillityUseSet playerAbillitySet;
        [SerializeField] protected PlayerAbilityPassiveSet playerAbillityPassiveSet;

        protected Player player;
        protected List<IPlayerTask> tasks = new List<IPlayerTask>();

        private HUDUI _hud;

        private PlayerContainer<DamageInfo> _playerDamageEffectContainer = new PlayerContainer<DamageInfo>();
        private MovementEffectContainer _playerEffectContainer = new MovementEffectContainer();

        public event Action DeadAction;

        protected event Action PlayAction;
        protected event Action StopAction;

        public int Health => _health.Health;
        public float MoveEffect => _playerEffectContainer.MoveEffect;
        public string Name => _name;
        protected bool IsPlay { get; private set; } = false;

        protected virtual void OnEnable()
        {
            _playerDamageEffectContainer.DeleteContentAction += (DamageInfo effect) => HideScreen(effect.Effect);
            _playerEffectContainer.DeleteContentAction += (MovementEffect effect) => HideScreen(effect.Effect);
        }

        protected virtual void OnDisable()
        {
            _playerDamageEffectContainer.DeleteContentAction -= (DamageInfo effect) => HideScreen(effect.Effect);
            _playerEffectContainer.DeleteContentAction -= (MovementEffect effect) => HideScreen(effect.Effect);
        }

        public void Start()
        {
            tasks.Add(_playerResist);
            tasks.Add(_playerEffectContainer);
            tasks.Add(_playerDamageEffectContainer);
        }

        public void Intializate(Player player, HUDUI hud)
        {
            this.player = player;
            _hud = hud;
            _hud.SetHealthPoint(_health.FullHealth);
            if (playerAbillitySet)
            {
                playerAbillitySet.SetHud(_hud);
                playerAbillitySet.SetUser(player);
            }
            if (playerAbillityPassiveSet)
            {
                playerAbillityPassiveSet.SetHud(_hud);
                playerAbillityPassiveSet.SetUser(player);
            }
        }

        public void Play()
        {
            IsPlay = true;
            Heal(_health.FullHealth);
            _animator.SetBool("Dead", false);
            if (playerAbillityPassiveSet)
                playerAbillityPassiveSet.Activate();
            if (PlayAction != null)
                PlayAction();
        }

        public void Stop()
        {
            IsPlay = false;
            _playerResist.Reset();
            _playerEffectContainer.Reset();
            if (playerAbillityPassiveSet)
                playerAbillityPassiveSet.Deactivate();
            if (StopAction != null)
                StopAction();
        }
        public void UpdateBehaviour()
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].Update();
            }
        }
        public void Dead()
        {
            _animator.SetBool("Dead", true);
        }
        public virtual void AddEffects(MovementEffect effect, float timeActive)
        {
            _playerEffectContainer.Add(effect, timeActive);
            if (_hud)
                _hud.ShowScreenEffect(effect.Effect);
            _animator.SetBool("Freez", effect.Effect == EffectType.Freez);
        }
        public virtual bool TakeDamage(int damage, DamageInfo damgeInfo)
        {
            if (!_playerResist.ContainResistAttack(damgeInfo.Attack))
            {
                _health.SetDamage(damage);
                if (_hud)
                    _hud.SetHealth(_health.Health);
                _playerDamageEffectContainer.Add(damgeInfo, damgeInfo.TimeEffect);
                _hud.ShowScreenEffect(damgeInfo.Effect);
                return true;
            }
            return false;
        }
        public bool Heal(int value)
        {
            if (_health.Heal(value))
            {
                if (_hud)
                    _hud.SetHealth(_health.Health);
                return true;
            }
            return false;
        }
        public PlayerAbillityUseSet UseAbillity()
        {
            return playerAbillitySet;
        }
        public void AddResist(DamageInfo damage, float timeActive)
        {
            _playerResist.Add(damage, timeActive);
        }
        private void DeadAnimationEvent()
        {
            if (DeadAction != null)
                DeadAction();
        }
        private void HideScreen(EffectType effect)
        {
            if (_hud)
                _hud.HideScreenEffect(effect);
            if(effect == EffectType.Freez)
                _animator.SetBool("Freez", false);
        }
    }
}
