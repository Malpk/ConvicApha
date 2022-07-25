using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Items;

namespace MainMode.Mode1921
{
    public class Point
    {
        private object _item;

        public bool IsBusy => _item != null;
        public Vector2 Position { get; private set; }

        public Point(Vector2 position)
        {
            Position = position;
        }

        public void SetItem(IMapItem item)
        {
            if (_item != null)
                RemoveObject();
            item.SetPosition(Position);
            _item = item;
        }
        public void SetItem(GameObject item)
        {
            _item = item;
            item.transform.position = Position;
        }
        private void RemoveObject()
        {
            if (_item is IMapItem item && item != null)
            {
                item.SetMode(false);
            }
            else if(_item is GameObject gameobject)
            {
                MonoBehaviour.Destroy(gameobject);
            }
            _item = null;
        }
    }
}