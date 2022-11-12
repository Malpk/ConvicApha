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
        [SerializeField] private PlayerAbillityUseSet _playerAbillitySet;
        [SerializeField] private PlayerAbilityPassiveSet _playerAbillityPassiveSet;

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

        private void OnEnable()
        {
            _playerDamageEffectContainer.DeleteContentAction += (DamageInfo effect) => HideScreen(effect.Effect);
            _playerEffectContainer.DeleteContentAction += (MovementEffect effect) => HideScreen(effect.Effect);
        }

        private void OnDisable()
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
            if (_playerAbillitySet)
            {
                _playerAbillitySet.SetHud(_hud);
                _playerAbillitySet.SetUser(player);
            }
            if (_playerAbillityPassiveSet)
            {
                _playerAbillityPassiveSet.SetHud(_hud);
                _playerAbillityPassiveSet.SetUser(player);
            }
        }

        public void Play()
        {
            IsPlay = true;
            Heal(_health.FullHealth);
            _animator.SetBool("Dead", false);
            if (_playerAbillityPassiveSet)
                _playerAbillityPassiveSet.Play();
            if (PlayAction != null)
                PlayAction();
        }

        public void Stop()
        {
            IsPlay = false;
            _playerResist.Reset();
            _playerEffectContainer.Reset();
            if (_playerAbillityPassiveSet)
                _playerAbillityPassiveSet.Stop();
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
        public void Heal(int value)
        {
            _health.Heal(value);
            if(_hud)
                _hud.SetHealth(_health.Health);
        }
        public void UseAbillity()
        {
            if (_playerAbillitySet)
                _playerAbillitySet.Use();
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
