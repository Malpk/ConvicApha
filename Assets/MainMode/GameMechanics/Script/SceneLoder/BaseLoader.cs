using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.GameInteface;
using PlayerComponent;
using UserIntaface.MainMenu;
using MainMode.Items;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;


namespace MainMode.LoadScene
{
    [RequireComponent(typeof(PlayerLoader), typeof(IntefaceLoader))]
    public class BaseLoader : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] protected bool playOnStart;
        [Header("Player Load Setting")]
        [SerializeField] protected Transform _spwanPoint;
        [SerializeField] protected PlayerConfig choosePlayer;
        [AssetReferenceUILabelRestriction("controller")]
        [SerializeField] private AssetReferenceGameObject _perfabAndroidController;

        protected PlayerLoader playerLoader;
        protected IntefaceLoader intefaceLoader;

        protected Player player;
        protected InterfaceSwitcher holder;

        private void Awake()
        {
            playerLoader = GetComponent<PlayerLoader>();
            intefaceLoader = GetComponent<IntefaceLoader>();
        }

        private async void Start()
        {
            if (playOnStart)
            {
                await LoadAsync(choosePlayer);
            }
        }
        #region LaodScene
        public async virtual Task LoadAsync(PlayerConfig config)
        {
            var list = new List<Task>();
            var loadReciver = intefaceLoader.LoadReceiverAsync();
            var loadInterface = intefaceLoader.LoadIntefaceAsync();
            await Task.WhenAll(loadReciver, loadInterface);
            var loadPlayer = playerLoader.PlayerLaodAsync(_spwanPoint, config.characterType);
            await loadPlayer;
            if (Application.platform == RuntimePlatform.Android)
                list.Add(_perfabAndroidController.LoadAssetAsync().Task);
            player = loadPlayer.Result;
            holder = loadInterface.Result;
            Intializate(config);
            intefaceLoader.UnloadReceiver();
            if (Application.platform == RuntimePlatform.Android)
                _perfabAndroidController.ReleaseAsset();
        }

        private void Intializate(PlayerConfig config)
        {
            var senders = player.GetComponents<ISender>();
            var hud = holder.GetComponentInChildren<HUDInteface>();
            if (hud)
            {
                for (int i = 0; i < senders.Length; i++)
                {
                    if (!hud.GetReceiver(senders[i]))
                    {
                        if (GetReceiverPerfab(senders[i].TypeDisplay, out Receiver perfab))
                            senders[i].AddReceiver(hud.CreateReceiver(perfab));
                    }
                }
            }
            SetController(player);
            if(config.itemConsumable != null && config.itemArtifact != null)
                player.AddDefaultItems(config.itemConsumable.GetComponent<ConsumablesItem>(), 
                    config.itemArtifact.GetComponent<Artifact>());
        }

        protected bool GetReceiverPerfab(TypeDisplay display, out Receiver perfab)
        {
            foreach (var receiver in intefaceLoader.Receivers)
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
                    if (_perfabAndroidController.Asset is GameObject perfab)
                    {
                        var controller = Instantiate(perfab, holder.transform);
                        player.SetController(controller.GetComponent<AndroidController>());
                    }
                    break;
                default:
                    player.SetController(player.gameObject.AddComponent<PcController>());
                    break;
            }
        }
        #endregion
    }
}