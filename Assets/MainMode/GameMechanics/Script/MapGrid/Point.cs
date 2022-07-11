using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Items;

namespace MainMode.Map
{
    public class Point
    {
        private GameObject _containeObject;

        public bool IsBusy => _containeObject;
        public Vector2 Position { get; private set; }

        public Point(Vector2 position)
        {
            Position = position;
        }

        public bool SetObject(GameObject instateObject)
        {
            if (_containeObject != null)
                return false;
            _containeObject = instateObject;
            _containeObject.transform.position = Position;
            return _containeObject;
        }
        public bool ChangeContainer(GameObject changeOnObject)
        {
            RemoveObject();
            return SetObject(changeOnObject);
        }

        private void RemoveObject()
        {
            if (_containeObject.TryGetComponent<Item>(out Item item))
                item.SetMode(false);
            else
                MonoBehaviour.Destroy(_containeObject);
            _containeObject = null;
        }
    }
}