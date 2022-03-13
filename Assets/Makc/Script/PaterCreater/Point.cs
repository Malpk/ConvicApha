using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class Point 
    {
        private readonly Vector3 _poistion;

        private ITileAnimation _aniamtion;
        private PoolTerm _tile;

        public Point(Vector3 position)
        {
            _poistion = position;
        }

        public bool IsBusy => _tile != null;
        public bool IsActive => _tile.IsActive;
        public Vector2 Position => _poistion;
        public ITileAnimation Animation => _aniamtion;

        public PoolTerm CreateObject(GameObject createObject)
        {
            if (!IsBusy)
            {
                var tile = MonoBehaviour.Instantiate(createObject, _poistion ,Quaternion.identity);
                _tile = tile.GetComponent<PoolTerm>();
                _aniamtion = _tile.GetComponent<ITileAnimation>();
            }
            return _tile;
        }

        public void DestroyObject()
        {
            MonoBehaviour.Destroy(_tile.gameObject);
        }

        public void SetAtiveObject(bool mode)
        {
            _tile.SetActiveMode(mode);
        }
    }
}