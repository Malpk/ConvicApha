using UnityEngine;
using MainMode.GameInteface;
using PlayerComponent;

namespace MainMode.LoadScene
{
    [CreateAssetMenu(menuName = "UIConfigs/PlayerInfo")]
    public class PlayerInfo : ScrollItem
    {
        [SerializeField] private PlayerBaseAbillitySet _prefabAbilluty;

        private PlayerBaseAbillitySet _abillityAseet;

        public PlayerBaseAbillitySet AddAbillity()
        {
            if (!_abillityAseet)
            {
                _abillityAseet = Instantiate(_prefabAbilluty.gameObject, asset.transform).
                    GetComponent<PlayerBaseAbillitySet>();
                return _abillityAseet;
            }
            else
            {
                return _abillityAseet;
            }
        }
    }
}