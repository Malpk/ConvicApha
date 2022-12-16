using UnityEngine;

namespace PlayerComponent
{
    public class PlayerEffectSet : MonoBehaviour, IAddEffects, IPlayerComponent
    {
        [SerializeField] private Player _player;

        private PlayerContainer<DamageInfo> _playerDamageEffectContainer = new PlayerContainer<DamageInfo>();
        private MovementEffectContainer _playerEffectContainer = new MovementEffectContainer();
        private PlayerContainer<EffectType> _screenEffect = new PlayerContainer<EffectType>();

        public System.Action<EffectType, bool> OnUpdateScreen;

        public float MoveEffect => _playerEffectContainer.MoveEffect;


        private void OnEnable()
        {
            _screenEffect.DeleteContentAction += DeleteEffect;
            _playerDamageEffectContainer.DeleteContentAction += (DamageInfo effect) => DeleteEffect(effect.Effect);
            _playerEffectContainer.DeleteContentAction += (MovementEffect effect) => DeleteEffect(effect.Effect);
        }
        private void OnDisable()
        {
            _screenEffect.DeleteContentAction -= DeleteEffect;
            _playerDamageEffectContainer.DeleteContentAction -= (DamageInfo effect) => DeleteEffect(effect.Effect);
            _playerEffectContainer.DeleteContentAction -= (MovementEffect effect) => DeleteEffect(effect.Effect);
        }
        private void Update()
        {
            _screenEffect.Update();
            _playerEffectContainer.Update();
            _playerDamageEffectContainer.Update();
        }

        public void Play()
        {
            _playerEffectContainer.Reset();
            _playerDamageEffectContainer.Reset();
            enabled = true;
        }
        public void Stop()
        {
            enabled = false;
        }
        public void AddEffect(MovementEffect info, float timeActive)
        {
            _playerEffectContainer.Add(info.Effect,info, timeActive);
            OnUpdateScreen?.Invoke(info.Effect, true);
            if (info.Effect == EffectType.Freez)
            {
                _player.SetState(PlayerState.Freez, true);
                _player.Block();
            }
        }
        public void AddEffectDamage(DamageInfo damage)
        {
            _playerDamageEffectContainer.Add(damage.Effect, damage, damage.TimeEffect);
            OnUpdateScreen?.Invoke(damage.Effect, true);
        }

        public void AddEffect(EffectType effect, float timeActive)
        {
            _screenEffect.Add(effect, effect,timeActive);
            OnUpdateScreen?.Invoke(effect, true);
        }
        private void DeleteEffect(EffectType effect)
        {
            OnUpdateScreen?.Invoke(effect, false);
            if (effect == EffectType.Freez)
            {
                _player.UnBlock();
                _player.SetState(PlayerState.Freez, false);
            }
        }

        public void ResetState()
        {
            _playerDamageEffectContainer.Reset();
            _playerEffectContainer.Reset();
        }
    }
}