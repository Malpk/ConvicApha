using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.GameInteface
{
    public class HUDInteface : UserInterface
    {
        private List<Receiver> _recirvers = new List<Receiver>();
        public override UserInterfaceType Type => UserInterfaceType.HUD;

        protected override void Awake()
        {
            base.Awake();
            var list = GetComponentsInChildren<Receiver>();
            if (list.Length > 0)
                _recirvers.AddRange(list);
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