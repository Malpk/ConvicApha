using UnityEngine;

namespace PlayerComponent
{
    public class PlayerEffectSet : MonoBehaviour, IAddEffects
    {
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
        public void AddEffects(MovementEffect effect, float timeActive)
        {
            _playerEffectContainer.Add(effect, timeActive);
            OnUpdateScreen?.Invoke(effect.Effect, true);
        }

        public void AddEffectDamage(DamageInfo damage)
        {
            _playerDamageEffectContainer.Add(damage, damage.TimeEffect);
            OnUpdateScreen?.Invoke(damage.Effect, true);
        }

        private void DeleteEffect(EffectType effect)
        {
            OnUpdateScreen?.Invoke(effect, false);
        }
    }
}