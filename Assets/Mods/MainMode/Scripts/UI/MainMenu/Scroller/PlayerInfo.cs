using UnityEngine;
using MainMode.GameInteface;

namespace MainMode.LoadScene
{
    [CreateAssetMenu(menuName = "UIConfigs/PlayerInfo")]
    public class PlayerInfo : ScrollItem
    {
        [SerializeField] private int _maxHealth;

        public int MaxHealth => _maxHealth;
    }
}