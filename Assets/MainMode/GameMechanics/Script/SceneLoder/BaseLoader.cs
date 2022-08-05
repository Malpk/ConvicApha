using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.GameInteface;
using PlayerComponent;
using UserIntaface.MainMenu;
using MainMode.Items;

namespace MainMode.LoadScene
{
    [RequireComponent(typeof(PlayerLoader), typeof(IntefaceLoader))]
    public class BaseLoader : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] protected bool playOnStart;
        [Header("Player Load Setting")]
        [SerializeField] protected Transform _spwanPoint;
        [SerializeField] protected PlayerType choosePlayer;
        public PlayerConfig Config;

        protected PlayerLoader playerLoader;
        protected IntefaceLoader intefaceLaoder;

        protected Player player;
        protected InterfaceSwitcher holder;

        [SerializeField] private AndroidController _perfabController;

        private void Awake()
        {
            playerLoader = GetComponent<PlayerLoader>();
            intefaceLaoder = GetComponent<IntefaceLoader>();
        }

        private void Start()
        {
            if (playOnStart)
            {
                Load(choosePlayer);
            }
        }

        public virtual void Load(PlayerType choose)
        {
            player = playerLoader.PlayerLaod(_spwanPoint ? _spwanPoint.position : Vector3.zero, choose);
            holder = intefaceLaoder.LaodInteface();
            var hud = holder.GetComponentInChildren<HUDInteface>();
            var senders = player.GetComponents<ISender>();
            for (int i = 0; i < senders.Length; i++)
            {
                if (!hud.GetReceiver(senders[i]))
                {
                    if (GetReceiverPerfab(senders[i].TypeDisplay, out Receiver perfab))
                        senders[i].AddReceiver(hud.CreateReceiver(perfab));
                }
            }
            SetController(player);
        }

        public virtual void Load(PlayerConfig config)
        {
            player = playerLoader.PlayerLaod(_spwanPoint ? _spwanPoint.position : Vector3.zero, config.characterType);
            holder = intefaceLaoder.LaodInteface();
            var hud = holder.GetComponentInChildren<HUDInteface>();
            var senders = player.GetComponents<ISender>();
            for (int i = 0; i < senders.Length; i++)
            {
                if (!hud.GetReceiver(senders[i]))
                {
                    if (GetReceiverPerfab(senders[i].TypeDisplay, out Receiver perfab))
                        senders[i].AddReceiver(hud.CreateReceiver(perfab));
                }
            }
            SetController(player);
            player.AddDefaultItems(config.itemConsumable.GetComponent<ConsumablesItem>(), config.itemArtifact.GetComponent<Artifact>());
        }

        protected bool GetReceiverPerfab(TypeDisplay display, out Receiver perfab)
        {
            foreach (var receiver in intefaceLaoder.Receivers)
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
        public void SetController(Player player)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    var controller = Instantiate(_perfabController.gameObject, holder.transform);
                    player.SetController(controller.GetComponent<AndroidController>());
                    break;
                default:
                    player.SetController(player.gameObject.AddComponent<PcController>());
                    break;
            }
        }
    }
}