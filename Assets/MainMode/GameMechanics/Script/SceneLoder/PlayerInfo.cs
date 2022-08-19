using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MainMode.LoadScene
{
    [CreateAssetMenu(menuName = "PlayerComponent/ PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [SerializeField] private PlayerType _type;
        [AssetReferenceUILabelRestriction("player")]
        [SerializeField] private AssetReferenceGameObject _playerPerfab;

        public PlayerType Type => _type;
        public AssetReferenceGameObject PlayerPerfab => _playerPerfab;
    }
}