using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using MainMode.GameInteface;

namespace MainMode
{
    public class PlayerScreen : MonoBehaviour, ISender
    {
        [SerializeField] private Sprite _freezSprite;
        [SerializeField] private SpriteRenderer _playerSpriteBody;
        [SerializeField] private Character _character;
        [SerializeField] private SwitchScreenEffect _screen;

        private Coroutine _corotine = null;

        public DamageInfo CurretAttack;

        public TypeDisplay TypeDisplay => TypeDisplay.ScreenUI;

        public bool AddReceiver(Receiver receiver)
        {
            if (_screen != null)
                return false;
            if (receiver is SwitchScreenEffect screen)
            {
                _screen = screen;
                return true;
            }
            else
            {
                return false; 
            }
        }
        public void ShowEffect(DamageInfo attack)
        {
            if (!_character.IsUseEffect)
                return;
            _screen.Show(attack.Effect, attack.TimeEffect);
            if (_corotine == null && attack.Effect == EffectType.Freez)
                _corotine = StartCoroutine(FreezeState(attack.TimeEffect));     
        }
        public void ShowEffect(EffectType type)
        {
            if (!_character.IsUseEffect)
                return;
            _screen.Show(type);
        }
        public void ScreenHide(EffectType type)
        {
            _screen.Hide(type);
        }
        private IEnumerator FreezeState(float duration)
        {
            var temp = _playerSpriteBody.sprite;
            _playerSpriteBody.sprite = _freezSprite;
            yield return new WaitForSeconds(duration);
            _playerSpriteBody.sprite = temp;
            _corotine = null;
        }

    }
}