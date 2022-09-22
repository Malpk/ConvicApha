using UnityEngine;
using MainMode.GameInteface;

namespace MainMode.LoadScene
{
    [CreateAssetMenu(menuName = "PlayerComponent/ PlayerInfo")]
    public class PlayerInfo : ScrollItem
    {
        [SerializeField] private PlayerType _type;

        public PlayerType Type => _type;

    }
}