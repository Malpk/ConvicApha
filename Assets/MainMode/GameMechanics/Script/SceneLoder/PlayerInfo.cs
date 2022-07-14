using UnityEngine;
using PlayerComponent;

namespace MainMode.LoadScene
{
    [System.Serializable]
    public class PlayerInfo
    {
#if UNITY_EDITOR
        [SerializeField] private string nameCell;
#endif
        [SerializeField] private PlayerType _type;
        [SerializeField] private Player _playerPerfab;

        public PlayerType Type => _type;
        public GameObject PlayerPerfab => _playerPerfab.gameObject;
    }
}