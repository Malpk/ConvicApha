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

        #region Load Interface
        public InterfaceSwitcher LoadInteface()
        {
            Holder = new GameObject("UserInterface").AddComponent<InterfaceSwitcher>();
            Holder.Intializate(GetIntefaces(Holder.transform), _startInterface);
            HUD = Holder.GetComponentInChildren<HUDInteface>();
            return Holder;
        }

        public void Intializate(Player player)
        {
            var senders = player.GetComponents<ISender>();
            if (HUD)
            {
                for (int i = 0; i < senders.Length; i++)
                {
                    if (!HUD.GetReceiver(senders[i]))
                    {
                        if (GetReceiverPerfab(senders[i].TypeDisplay, out Receiver perfab))
                            senders[i].AddReceiver(HUD.CreateReceiver(perfab));
                    }
                }
                Holder.SetShow(HUD);
            }
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