using UnityEngine;
using PlayerComponent;

namespace MainMode.LoadScene
{
    public class PlayerLoader : MonoBehaviour
    {
        [SerializeField] private PlayerInfo[] _playerPerfabs;
        [SerializeField] private AndroidController _perfabAndroidController;

        public delegate void Loading(Player player);
        public event Loading PlayerLoadAction;

        public Player Player { get; private set; }
        public Controller Controller { get; private set; }

        public void PlayerLoad(Transform spawnPosition, PlayerConfig config)
        {
            var player = GetPlayer(config.Type);
            if (player != Player)
            {
                if(Player)
                    Player.gameObject.SetActive(false);
                Player = player;
                if (PlayerLoadAction != null)
                    PlayerLoadAction(player);
            }
            Player.transform.position = spawnPosition ? spawnPosition.position : Vector3.zero;
            if (Player.TryGetComponent(out PlayerInventory inventorySet))
            {
                if (config.ItemArtifact)
                {
                    config.ItemArtifact.Pick(Player);
                    inventorySet.AddArtifact(config.ItemArtifact);
                }
                if (config.ItemConsumable)
                {
                    config.ItemConsumable.Pick(Player);
                    inventorySet.AddConsumablesItem(config.ItemConsumable);
                }
            }
        }
        private Player GetPlayer(PlayerType type)
        {
            foreach (var config in _playerPerfabs)
            {
                if (config.Type == type)
                {
                    return config.Create<Player>();
                }
            }
            return null;
        }
    }
}