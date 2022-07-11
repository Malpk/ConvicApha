using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.GameInteface
{
    public class InterfaceSwitcher : MonoBehaviour
    {
        [SerializeField] private UserInterface _curretInteface;
        [SerializeField] private UserInterface[] _intefaces;

        private List<UserInterface> _stack = new List<UserInterface>();

        public void Start()
        {
            foreach (var interfaceItem in _intefaces)
            {
                interfaceItem.Intializate(this);
            }
        }
        public void SetShow(UserInterface inteface)
        {
            if (_curretInteface == inteface || inteface == null)
                return;
            UpdateStack(_curretInteface);
            SetHide(_curretInteface);
            _curretInteface = inteface;
            _curretInteface.OnShow();
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