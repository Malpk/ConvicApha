using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.GameInteface
{
    public class HUDInteface : UserInterface
    {
        [SerializeField] private Canvas _canvas;

        private List<Receiver> _recirvers = new List<Receiver>();
        public override UserInterfaceType Type => UserInterfaceType.HUD;

        protected void Awake()
        {
            var list = GetComponentsInChildren<Receiver>();
            if (list.Length > 0)
                _recirvers.AddRange(list);
        }

        private void OnEnable()
        {
            ShowAction += () => _canvas.enabled = true;
            HideAction += () => _canvas.enabled = false;
        }

        private void OnDisable()
        {
            ShowAction -= () => _canvas.enabled = true;
            HideAction -= () => _canvas.enabled = false;
        }

        public bool GetReceiver(ISender sender)
        {
            foreach (var receiver in _recirvers)
            {
                if (receiver.DisplayType == sender.TypeDisplay)
                {
                    sender.AddReceiver(receiver);
                    return true;
                }
            }
            return false;
        }
        public Receiver CreateReceiver(Receiver perfab)
        {
            foreach (var element in _recirvers)
            {
                if (element.DisplayType == perfab.DisplayType)
                    return element;
            }
            var receiver = Instantiate(perfab.gameObject, transform).GetComponent<Receiver>();
            _recirvers.Add(receiver);
            return receiver;
        }
    }
}