using System.Collections;
using System.Collections.Generic;
using MainMode.GameInteface;
using UnityEngine;

namespace MainMode.LoadScene
{
    public class IntefaceLoader : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] private UserInterfaceType _startInterface;
        [Header("Requred Perfab")]
        [SerializeField] private Receiver[] _reciverPerfabs;
        [SerializeField] private UserInterface[] _userInterfacePerfab;

        public InterfaceSwitcher Holder { get; private set; }
        public HUDInteface HUD { get; private set; }

        public DeadMenu DeadMenu { get; private set; }

        #region Load Interface
        public InterfaceSwitcher LoadInteface()
        {
            if (!Holder)
            {
                Holder = new GameObject("UserInterface").AddComponent<InterfaceSwitcher>();
                Holder.Intializate(GetIntefaces(Holder.transform), _startInterface);
                HUD = Holder.GetComponentInChildren<HUDInteface>();
                DeadMenu = Holder.GetComponentInChildren<DeadMenu>();
            }
            return Holder;
        }
        private UserInterface[] GetIntefaces(Transform holder)
        {
            var list = new List<UserInterface>();
            foreach (var perfab in _userInterfacePerfab)
            {
                list.Add(Instantiate(perfab.gameObject, holder).GetComponent<UserInterface>());
            }
            return list.ToArray();
        }
        protected bool GetReceiverPerfab(TypeDisplay display, out Receiver perfab)
        {
            foreach (var receiver in _reciverPerfabs)
            {
                if (receiver.DisplayType == display)
                {
                    perfab = receiver;
                    return true;
                }
            }
            perfab = null;
            return false;
        }
        #endregion
    }
}