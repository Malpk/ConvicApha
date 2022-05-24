using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode
{
    [RequireComponent(typeof(Animator))]
    public class SwitchScreenEffect : MonoBehaviour
    {
        [SerializeField] private Image[] _cellScreen;
        [SerializeField] private ScreenEffect[] _screens;

        private Animator _animator;
        private Dictionary<EffectType, int> _screenActive = new Dictionary<EffectType, int>();
        private Dictionary<EffectType, Image> _screenReference = new Dictionary<EffectType, Image>();
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            foreach (var image in _cellScreen)
            {
                image.sprite = null;
                image.enabled = false;
            }
        }

        public void SetScreen(EffectType type, float duration = 1)
        {
            if (!_screenActive.ContainsKey(type))
            {
                 ScreenOn(type);
            }
            else
            {
                _screenActive[type]++;
            }
            StartCoroutine(ScreenOff(type, duration));
        }
        public void SetScreen(EffectType type)
        {
            switch (type)
            {
                case EffectType.Flash:
                    _animator.SetTrigger("flash");
                    break;
                default:
                    if (_screenActive.ContainsKey(type))
                        _screenActive[type]++;
                    else
                        ScreenOn(type);
                    break;
            }
        }
        private IEnumerator ScreenOff(EffectType type, float duration)
        {
            yield return new WaitForSeconds(duration);
            ScreenOff(type);
        }
        public void ScreenOff(EffectType type)
        {
            if (!_screenActive.ContainsKey(type))
                return;
            _screenActive[type]--;
            if (_screenActive[type] == 0)
            {
                _screenReference[type].enabled = false;
                _screenReference[type].sprite = null;
                _screenReference.Remove(type);
                _screenActive.Remove(type);
            }
        }
        private void ScreenOn(EffectType type)
        {
            var screen = ChooseCellScreen(type);
            if (screen != null)
            {
                _screenReference.Add(type, screen);
                _screenActive.Add(type, 1);
                screen.enabled = true;
            }
        }
        private Image ChooseCellScreen(EffectType type)
        {
            var screen = GetScreen(type);
            if (screen == null)
                return null;
            foreach (var cell in _cellScreen)
            {
                if (cell.sprite == null)
                {
                    cell.sprite = screen;
                    cell.transform.parent = null;
                    cell.transform.parent = transform;
                    return cell;
                }
            }
            return null;
        }
        private Sprite GetScreen(EffectType type)
        {
            foreach (var screen in _screens)
            {
                if (screen.Type == type)
                    return screen.Screen;
            }
            return null;
        }
    }
}