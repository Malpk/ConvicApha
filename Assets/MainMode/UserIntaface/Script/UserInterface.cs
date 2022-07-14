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
            _parentCanvas.enabled = true;
        }
        public void Hide()
        {
            _parentCanvas.enabled = false;
        }

        public GameObject InstateElement(GameObject element)
        {
            return element;
        }
    }
}