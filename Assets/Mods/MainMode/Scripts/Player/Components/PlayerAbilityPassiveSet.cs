using UnityEngine;
using System.Collections;
using MainMode.GameInteface;

namespace PlayerComponent
{
    public abstract class PlayerAbilityPassiveSet : MonoBehaviour, IPlayerAbillitySet
    {
        [Header("Ability Setting")]
        [Min(1)]
        [SerializeField] private int _timeReload = 1;
        [SerializeField] protected Player user;
        [SerializeField] protected Sprite _abillityIcon;

        private bool _isPlay = false;

        protected HUDUI hud;

        public void SetHud(HUDUI hud)
        {
            this.hud = hud;
            hud.SetAbilityIcon(_abillityIcon, false);
        }

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
                hud.DisplayStateAbillity(true);
                yield return new WaitForSeconds(0.2f);
                hud.DisplayStateAbillity(false);
                int progress = _timeReload;
                while (progress > 0 && _isPlay)
                {
                    hud.UpdateAbillityKdTimer(progress);
                    yield return new WaitForSeconds(1f);
                    progress--;
                }
                UseAbility();
            }
        }
    }
}
