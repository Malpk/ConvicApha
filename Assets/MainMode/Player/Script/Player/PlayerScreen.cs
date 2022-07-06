using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class PlayerScreen : MonoBehaviour, IEffect
    {
        [SerializeField] private Sprite _freezSprite;
        [SerializeField] private SpriteRenderer _playerSpriteBody;
        [SerializeField] private Character _character;

        [SerializeField] private SwitchScreenEffect _screen;

        private Coroutine _corotine = null;

        public void ShowEffect(AttackInfo attack)
        {
            if (!_character.IsUseEffect)
                return;
            _screen.SetScreen(attack.Effect, attack.TimeEffect);
            if (_corotine == null && attack.Effect == EffectType.Freez)
                _corotine = StartCoroutine(FreezeState(attack.TimeEffect));     
        }
        public void ShowEffect(EffectType type)
        {
            if (!_character.IsUseEffect)
                return;
            _screen.SetScreen(type);
        }
        public void ScreenHide(EffectType type)
        {
            _screen.ScreenHide(type);
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