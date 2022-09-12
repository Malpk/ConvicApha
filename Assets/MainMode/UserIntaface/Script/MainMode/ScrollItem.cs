using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MainMode.GameInteface
{
    public class ScrollItem : ScriptableObject
    {
        [SerializeField] private Sprite _itemImage;
        [SerializeField] private string _loadKey;
        [SerializeField] private AssetReferenceGameObject _playerPerfab;
        
        public string LoadKey => _loadKey;
        public Sprite ItemImage => _itemImage;
        public AssetReferenceGameObject PlayerPerfab => _playerPerfab;
    }
}
