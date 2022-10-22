using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MainMode.GameInteface
{
    public class InterfaceSwitcher : MonoBehaviour
    {
        private Player _player;
        private DeadMenu _deadMenu;

        private UserInterface _curretInteface;
        private UserInterface[] _intefaces;

        private List<UserInterface> _stack = new List<UserInterface>();

        [Inject]
        public void Construct(Player player, DeadMenu deadMenu)
        {
            _player = player;
            _deadMenu = deadMenu;
        }

        public void Intializate(UserInterface[] intefaces, UserInterfaceType startInterface)
        {
            _intefaces = intefaces;
            for (int i = 0; i < _intefaces.Length; i++)
            {
                _intefaces[i].Intializate(this);
                _intefaces[i].Hide();
            }
            foreach (var element in _intefaces)
            {
                if (element.Type == startInterface)
                {
                    _curretInteface = element;
                    _curretInteface.Show();
                }
            }
        }

        private void OnEnable()
        {
            _player.DeadAction += () => SetShow(GetComponentInChildren<DeadMenu>());
            _deadMenu.RestartAction += () => SetShow(GetComponentInChildren<HUDInteface>());
        }
        private void OnDisable()
        {
            _player.DeadAction -= () => SetShow(GetComponentInChildren<DeadMenu>());
            _deadMenu.RestartAction -= () => SetShow(GetComponentInChildren<HUDInteface>());
        }

        public void SetShow(UserInterface inteface)
        {
            if (_curretInteface == inteface)
                return;
            SetHide(_curretInteface);
            UpdateStack(_curretInteface);
            _curretInteface = inteface;
            _curretInteface.Show();
        }
        public void SetHide()
        {
            if (_stack.Count > 0)
            {
                SetShow(_stack[_stack.Count - 1]);
                _stack.Remove(_curretInteface);
            }
        }
        public void SetHide(UserInterface inteface)
        {
            if (inteface != null)
                inteface.Hide();
        }
        private void UpdateStack(UserInterface inteface)
        {
            if (inteface == null)
                return;
            if (!_stack.Contains(inteface))
            {
                _stack.Add(inteface);
            }
            else
            {
                for (int i = _stack.IndexOf(inteface); i < _stack.Count - 1; i++)
                {
                    var temp = _stack[i];
                    _stack[i] = _stack[i + 1];
                    _stack[i + 1] = temp;
                }
            }
        }
    }
}