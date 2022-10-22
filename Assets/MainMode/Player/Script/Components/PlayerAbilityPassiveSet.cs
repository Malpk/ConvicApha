using UnityEngine;
using System.Collections;

namespace PlayerComponent
{
    public abstract class PlayerAbilityPassiveSet : MonoBehaviour, IPlayerAbillitySet
    {
        [Header("Ability Setting")]
        [Min(1)]
        [SerializeField] private float _timeReload = 1f;

        private bool _isPlay = false;

        protected Player user;

        public void Play()
        {
            if (!_isPlay)
            {
                _isPlay = true;
                StartCoroutine(Use());
            }
        }

        public void Stop()
        {
            _isPlay = false;
        }
        public void SetUser(Player player)
        {
            user = player;
        }

        protected abstract void UseAbility();

        private IEnumerator Use()
        {
            while (_isPlay)
            {
                var progress = 0f;
                while (progress<=1f && _isPlay)
                {
                    progress += Time.deltaTime/ _timeReload;
                    yield return null;
                }
                UseAbility();
            }
        }
    }
}
