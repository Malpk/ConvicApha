using MainMode.GameInteface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Mode1921
{
    public class ToolSet : MonoBehaviour,ISender
    {
        [SerializeField] private ToolDisplay _toolDisplay;

        private int _countTools;

        public int CountTools => _countTools;

        public TypeDisplay TypeDisplay => TypeDisplay.ToolSetUI;

        public bool AddReceiver(Receiver receiver)
        {
            if (_toolDisplay != null)
                return false;
            if (receiver is ToolDisplay display)
                _toolDisplay = display;
            return _toolDisplay;
        }

        public void ShowHint()
        {
            _toolDisplay.ShowHint();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out ToolRepairs tool))
            {
                _countTools++;
                if (_toolDisplay != null)
                    _toolDisplay.Display(tool.Icon);
                tool.Pick();
            }
        }
    }
}
