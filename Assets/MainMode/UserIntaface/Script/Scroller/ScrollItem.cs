using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.GameInteface
{
    [CreateAssetMenu(menuName = "UIConfigs/ItenConfig")]
    public class ScrollItem : ScriptableObject
    {
        [SerializeField] private Sprite _itemImage;
        [SerializeField] private string _loadKey;
        
        public string LoadKey => _loadKey;
        public Sprite ItemImage => _itemImage;
    }
}
