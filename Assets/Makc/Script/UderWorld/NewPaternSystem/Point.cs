using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class Point : IPoint
    {
        private readonly Vector3 _poistion;

        private ITileAnimation _aniamtion;
        private GameObject _tile;

        public Point(Vector3 position)
        {
            _poistion = position;
        }

        public bool IsBusy => _tile != null;
        public ITileAnimation Animation => _aniamtion;

        public GameObject CreateObject(GameObject createObject)
        {
            if (!IsBusy)
            {
                _tile = MonoBehaviour.Instantiate(createObject, _poistion ,Quaternion.identity);
                _aniamtion = _tile.GetComponent<ITileAnimation>();
            }
            return _tile;
        }

        public void DestroyObject()
        {
            MonoBehaviour.Destroy(_tile);
        }

        public void SetAtiveObject(bool mode)
        {
            if(_tile.activeSelf != mode)
                _tile.SetActive(mode);
        }
    }
}