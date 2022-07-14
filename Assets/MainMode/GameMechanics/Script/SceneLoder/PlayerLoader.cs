using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;

namespace MainMode.LoadScene
{
    public class PlayerLoader : MonoBehaviour
    {
        [SerializeField] private PlayerInfo[] _playerPerfabs;
        [SerializeField] private CameraFollowing _cameraPerfab;
        [SerializeField] private CameraFollowing _cameraFollowing;

        public Player PlayerLaod(Vector2 spawnPosition, PlayerType playerType = PlayerType.None)
        {
            if (GetPerafab(playerType, out GameObject prefab))
            {
                var player = MonoBehaviour.Instantiate(prefab, spawnPosition, Quaternion.identity).GetComponent<Player>();
               if(_cameraFollowing == null)
                    _cameraFollowing = MonoBehaviour.Instantiate(_cameraPerfab.gameObject,
                        new Vector3(spawnPosition.x,spawnPosition.y, _cameraPerfab.transform.position.z), 
                            Quaternion.identity).GetComponent<CameraFollowing>();
                _cameraFollowing.SetTarget(player);
                return player;
            }
            else
            {
                return null;
            }
        }
     
        private bool GetPerafab(PlayerType type,out GameObject perfab)
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