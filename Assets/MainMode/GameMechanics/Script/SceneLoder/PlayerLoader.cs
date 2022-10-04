using UnityEngine;
using PlayerComponent;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace MainMode.LoadScene
{
    public class PlayerLoader : MonoBehaviour
    {
        [SerializeField] private PlayerInfo[] _playerPerfabs;
        [AssetReferenceUILabelRestriction("controller")]
        [SerializeField] private AssetReferenceGameObject _perfabAndroidController;

        private PlayerType _type;

        public delegate void Loading(Player player);
        public event Loading PlayerLoadAction;

        public Player Player { get; private set; }
        public Controller Controller { get; private set; }

        public async Task<Player> PlayerLaodAsync(Transform spawnPosition, PlayerConfig config)
        {
            if (_type != config.type || !Player)
            {
                _type = config.type;
                UnLoadPLayer();
                var task = LoadPlayer(config.type);
                await task;
                Player = task.Result.GetComponent<Player>();
                SetController(Player);
                if (PlayerLoadAction != null)
                {
                    PlayerLoadAction(Player);
                }
            }
            Player.AddDefaultItems(config.itemConsumable, config.itemArtifact);
            Player.transform.position = spawnPosition ? spawnPosition.position : Vector3.zero;
            return Player;
        }
        private async Task<Player> LoadPlayer(PlayerType playerType)
        {
            if (GetLoadKey(playerType, out string loadKey))
            {
                var loadPlayer = Addressables.InstantiateAsync(loadKey).Task;
                await loadPlayer;
                var player = loadPlayer.Result.GetComponent<Player>();
                return player;
            }
            return null;
        }
        public void UnLoadPLayer()
        {
            if(Player)
                Addressables.ReleaseInstance(Player.gameObject);
            if (Application.platform == RuntimePlatform.Android)
                _perfabAndroidController.ReleaseAsset();
        }
  
        private bool GetLoadKey(PlayerType type, out string loadKey)
        {
            foreach (var player in _playerPerfabs)
            {
                if (player.Type == type)
                {
                    loadKey = player.LoadKey;
                    return true;
                }
            }
            loadKey = null;
            return false;
        }
        private void SetController(Player player)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    if (_perfabAndroidController.Asset is GameObject perfab)
                    {
                        Controller = Instantiate(perfab).GetComponent<AndroidController>();
                    }
                    else
                    {
                        Controller = null;
                    }
                    break;
                default:
                    Controller = player.gameObject.AddComponent<PcController>();
                    break;
            }
            player.SetControoler(Controller);
        }
    }
}