using UnityEngine;

namespace PlayerComponent
{
    public class PlayerEffectSet : MonoBehaviour, IAddEffects, IPlayerComponent
    {
        [SerializeField] private Player _player;
        [SerializeField] private PlayerMovementSet _movement;

        private PlayerContainer<DamageInfo> _playerDamageEffectContainer = new PlayerContainer<DamageInfo>();
        private MovementEffectContainer _playerEffectContainer = new MovementEffectContainer();

        public System.Action<EffectType, bool> OnUpdateScreen;

        public float MoveEffect => _playerEffectContainer.MoveEffect;


        private void OnEnable()
        {
            _playerDamageEffectContainer.DeleteContentAction += (DamageInfo effect) => DeleteEffect(effect.Effect);
            _playerEffectContainer.DeleteContentAction += (MovementEffect effect) => DeleteEffect(effect.Effect);
        }
        private void OnDisable()
        {
            _playerDamageEffectContainer.DeleteContentAction -= (DamageInfo effect) => DeleteEffect(effect.Effect);
            _playerEffectContainer.DeleteContentAction -= (MovementEffect effect) => DeleteEffect(effect.Effect);
        }
        private void Update()
        {
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
        public void AddEffects(MovementEffect effect, float timeActive)
        {
            _playerEffectContainer.Add(effect, timeActive);
            OnUpdateScreen?.Invoke(effect.Effect, true);
            if (effect.Effect == EffectType.Freez)
            {
                _player.SetState(PlayerState.Freez, true);
                _player.Block();
            }
        }
        public void AddEffectDamage(DamageInfo damage)
        {
            _playerDamageEffectContainer.Add(damage, damage.TimeEffect);
            OnUpdateScreen?.Invoke(damage.Effect, true);
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
    }
}