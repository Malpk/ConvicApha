using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Items
{   
    public abstract class Item : MonoBehaviour, IPickable, IUseable
    {

        [SerializeField] protected ItemEffect _itemEffect;
        [SerializeField] protected Sprite ItemSprite;

        protected Player _ownerPlayer;
        public Sprite Sprite => ItemSprite;
        public ItemEffect Effect { get => _itemEffect;}
        private void Awake()
        {
            _itemEffect = GetComponent<ItemEffect>();
        }
        public abstract void Pick(Player player);
        public abstract void Use();
    }
}