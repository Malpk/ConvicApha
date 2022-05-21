using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseMode
{
    public class PlayerEffect : MonoBehaviour
    {
        [SerializeField] private Sprite _freezSprite;
        [SerializeField] private SpriteRenderer _playerSpriteBody;

        [SerializeField] private SwitchScreenEffect _screen;

        private Coroutine _corotine = null;

        public void SetEffect(TrapType type, float duration)
        {

            switch (type)
            {
                case TrapType.U92:
                    _screen.SetScreen(type);
                    break;
                default:
                    _screen.SetScreen(type, duration);
                    break;
            }
            if (_corotine == null && type == TrapType.N7)
                _corotine = StartCoroutine(FreezeState(duration));     
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