using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.GameInteface
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UserInterface : MonoBehaviour
    {
        public abstract UserInterfaceType Type { get; }
        public bool IsShow { get; private set; }

        protected event System.Action HideAction;
        protected event System.Action ShowAction;

        public void Show()
        {
            IsShow = true;
            if (ShowAction != null)
                ShowAction();
        }
        public void Hide()
        {
            IsShow = false;
            if (HideAction != null)
                HideAction();
        }

        public GameObject InstateElement(GameObject element)
        {
            return element;
        }
    }
}