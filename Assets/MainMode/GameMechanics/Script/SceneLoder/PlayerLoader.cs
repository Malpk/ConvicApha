using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

namespace MainMode.LoadScene
{
    public class PlayerLoader : MonoBehaviour
    {
        [SerializeField] private PlayerInfo[] _playerPerfabs;

        public async Task<Player> PlayerLaodAsync(Transform spawnPosition, PlayerType playerType = PlayerType.None)
        {
            if (GetPerafab(playerType, out AssetReferenceGameObject prefab))
            {
                var position = spawnPosition ? spawnPosition.position : Vector3.zero;
                var loadPlayer = prefab.InstantiateAsync(position, Quaternion.identity).Task;
                await loadPlayer;
                var player = loadPlayer.Result.GetComponent<Player>();
                return player;
            }
            else
            {
                return null;
            }
        }

            
        private bool GetPerafab(PlayerType type,out AssetReferenceGameObject perfab)
        {
            foreach (var player in _playerPerfabs)
            {
                if (player.Type == type)
                {
                    perfab = player.PlayerPerfab;
                    return true;
                }
            }
            perfab = null;
            return false;
        }
    }
}