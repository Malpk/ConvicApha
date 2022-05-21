using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaseMode
{
    [RequireComponent(typeof(Animator))]
    public class SwitchScreenEffect : MonoBehaviour
    {
        [SerializeField] private Image[] _cellScreen;
        [SerializeField] private ScreenEffect[] _screens;

        private Animator _animator;
        private Dictionary<TrapType,float> _screenActive = new Dictionary<TrapType,float>();

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            foreach (var image in _cellScreen)
            {
                image.sprite = null;
                image.enabled = false;
            }
        }

        public void SetScreen(TrapType type, float duration = 1)
        {
            if (!_screenActive.ContainsKey(type))
            {
                _screenActive.Add(type, duration);
                StartCoroutine(ScreenWork(type));
            }
            else
            {
                _screenActive[type] = duration;
            }
        }
        public void SetScreen(TrapType type)
        {
            switch (type)
            {
                case TrapType.U92:
                    _animator.SetTrigger("flash");
                    break;
            }
        }
        private IEnumerator ScreenWork(TrapType type)
        {
            var screen = ChooseCellScreen(type);
            if (screen != null)
            {
                screen.enabled = true;
                while (_screenActive[type] > 0)
                {
                    _screenActive[type] -= Time.deltaTime;
                    yield return null;
                }
                screen.enabled = false;
            }
            _screenActive.Remove(type);
        }
        private Image ChooseCellScreen(TrapType type)
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
        private Sprite GetScreen(TrapType type)
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