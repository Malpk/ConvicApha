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
        [SerializeField] protected bool autoRestart;
        [Header("Player Load Setting")]
        [SerializeField] protected Transform _spwanPoint;
        [SerializeField] protected PlayerConfig choosePlayer;
        [SerializeField] protected CameraFollowing _cameraFollowing;
        [AssetReferenceUILabelRestriction("controller")]
        [SerializeField] private AssetReferenceGameObject _perfabAndroidController;

        protected PlayerLoader playerLoader;
        protected IntefaceLoader intefaceLoader;

        protected Player player;
        protected InterfaceSwitcher holder;

        private string _cameraLoadKey = "CameraFollowing";

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
            var loadCamera = LoadCameraFollowingAsync();
            await Task.WhenAll(loadPlayer, loadCamera);
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
            _cameraFollowing.SetTarget(player);
            var marker = hud.GetComponentInChildren<MarkerUI>();
            if (marker)
            {
                marker.Intilizate(player, _cameraFollowing);
                marker.Play();
            }
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
            SetController(out Controller controller);
            player.Intiliazate(controller, marker);
            if (config.itemConsumable != null && config.itemArtifact != null)
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
        public bool SetController(out Controller controller)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    if (_perfabAndroidController.Asset is GameObject perfab)
                    {
                        controller = Instantiate(perfab, holder.transform).GetComponent<AndroidController>();
                    }
                    else
                        controller = null;
                    break;
                default:
                    controller = player.gameObject.AddComponent<PcController>();
                    break;
            }
            return controller;
        }
        #endregion
        private async Task LoadCameraFollowingAsync()
        {
            try
            {
                if (_cameraFollowing == null)
                {
                    var loadCamera = LoadCamera(_cameraLoadKey);
                    await loadCamera;
                    _cameraFollowing = loadCamera.Result;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            _cameraFollowing.transform.position = _spwanPoint ?  new Vector3(_spwanPoint.position.x,
                _spwanPoint.position.y, _cameraFollowing.transform.position.z) :
                    Vector3.forward * _cameraFollowing.transform.position.z;
        }
        private async Task<CameraFollowing> LoadCamera(string keyLoad)
        {
            var load = Addressables.InstantiateAsync(keyLoad).Task;
            await load;
            var cameraFollowing = load.Result;
            try
            {
                if (!cameraFollowing.TryGetComponent(out CameraFollowing camera))
                    throw new System.Exception("GameObject is not component CameraFollowing");
                return camera;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}