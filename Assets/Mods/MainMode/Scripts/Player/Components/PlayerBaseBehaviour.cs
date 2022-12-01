using System.Collections.Generic;
using MainMode.GameInteface;
using UnityEngine;
using System;

namespace PlayerComponent
{
    public class PlayerBaseBehaviour : MonoBehaviour
    {
        [SerializeField] private int _fullHealth;
        [SerializeField] private string _name;
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerResistContainer _playerResist;

        [SerializeField] protected PlayerAbillityUseSet playerAbillitySet;
        [SerializeField] protected PlayerAbilityPassiveSet playerAbillityPassiveSet;

        protected Player player;
        protected List<IPlayerTask> tasks = new List<IPlayerTask>();

        private HUDUI _hud;


        public event Action DeadAction;

        protected event Action PlayAction;
        protected event Action StopAction;

        public int Health { get; private set; }

        public string Name => _name;
        protected bool IsPlay { get; private set; } = false;

    
        public void Start()
        {
            tasks.Add(_playerResist);
        }

        public void Intializate(Player player, HUDUI hud, int health)
        {
            _fullHealth = health;
            this.player = player;
            _hud = hud;
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
            Heal(_fullHealth);
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
            _animator.SetBool("Freez", effect.Effect == EffectType.Freez);
        }
        public virtual bool TakeDamage(int damage, DamageInfo damgeInfo)
        {
            if (!_playerResist.ContainResistAttack(damgeInfo.Attack))
            {
                Health = Mathf.Clamp(Health - damage, 0, _fullHealth);
                return true;
            }
            return false;
        }
        public bool Heal(int value)
        {
            if (Health < _fullHealth  )
            {
                Health =Mathf.Clamp(Health + value, 0, _fullHealth);
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

    }
}
