using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class PlayerScreen : MonoBehaviour, IEffect
    {
        [SerializeField] private Sprite _freezSprite;
        [SerializeField] private SpriteRenderer _playerSpriteBody;

        [SerializeField] private SwitchScreenEffect _screen;

        private Coroutine _corotine = null;

        public void SetEffect(EffectType type, float duration)
        {
            _screen.SetScreen(type, duration);
            if (_corotine == null && type == EffectType.Freez)
                _corotine = StartCoroutine(FreezeState(duration));     
        }
        public void SetEffect(EffectType type)
        {
            _screen.SetScreen(type);
        }
        public void ScreenOff(EffectType type)
        {
            _screen.ScreenOff(type);
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