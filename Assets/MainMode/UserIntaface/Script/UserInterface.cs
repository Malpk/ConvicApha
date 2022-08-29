using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.GameInteface
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UserInterface : MonoBehaviour
    {
        protected Canvas _parentCanvas;
        protected InterfaceSwitcher swithchInteface;
     
        public abstract UserInterfaceType Type { get; }
        public bool IsShow { get; private set; }

        protected event System.Action HideAction;

        protected virtual void Awake()
        {
            _parentCanvas = GetComponent<Canvas>();
        }
        public void Intializate(InterfaceSwitcher swither)
        {
            swithchInteface = swither;
        }
        public void OnShow()
        {
            IsShow = true;
            _parentCanvas.enabled = true;
        }
        public void Hide()
        {
            IsShow = false;
            _parentCanvas.enabled = false;
            if (HideAction != null)
                HideAction();
        }

        public GameObject InstateElement(GameObject element)
        {
            return element;
        }
    }
}