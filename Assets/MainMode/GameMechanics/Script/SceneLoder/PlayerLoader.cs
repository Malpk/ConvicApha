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
        [SerializeField] private CameraFollowing _cameraFollowing;
        [AssetReferenceUILabelRestriction("default")]
        [SerializeField] private AssetReferenceGameObject _cameraPerfab;

        public async Task<Player> PlayerLaodAsync(Transform spawnPosition, PlayerType playerType = PlayerType.None)
        {
            if (GetPerafab(playerType, out AssetReferenceGameObject prefab))
            {
                var position = spawnPosition ? spawnPosition.position : Vector3.zero;
                var loadPlayer = prefab.InstantiateAsync(position, Quaternion.identity).Task;
                await loadPlayer;
                var player = loadPlayer.Result.GetComponent<Player>();
                try
                {
                    if (_cameraFollowing == null)
                    {
                        var loadCamera = LoadCamera(_cameraPerfab);
                        await loadCamera;
                        _cameraFollowing = loadCamera.Result;
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                _cameraFollowing.transform.position = new Vector3(position.x, 
                    position.y, _cameraFollowing.transform.position.z);
                _cameraFollowing.SetTarget(player);
                return player;
            }
            else
            {
                return null;
            }
        }
        private async Task<CameraFollowing> LoadCamera(AssetReferenceGameObject perfab)
        {
            var load = perfab.InstantiateAsync().Task;
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