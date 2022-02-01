using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using SwitchMode;

namespace Trident 
{
    public class TridentMode : MonoBehaviour, ISequence
    {
        [Header("Game Setting")]
        [SerializeField] private int _offset;
        [SerializeField] private float _delay;
        [SerializeField] private float _duration;
        [SerializeField] private float _warningTime;
        [SerializeField] private GameObject _trident;
        [Header("Perfab Setting")]
        [SerializeField] private Vector2Int _sizeMap;
        [SerializeField] private Mode _setting;
        [SerializeField] private GameObject _spawner;
        [SerializeField] private MarkerSetting[] _tridentSetting;

        private Tilemap _tileMap;
        private Coroutine _coroutine;

        public void Constructor(SwitchMods swictMode)
        {
            if (_coroutine != null)
                return;
            _tileMap = swictMode.tileMap;
            Intializate();
            _coroutine = StartCoroutine(RunMode());
        }

        private void Intializate()
        {
            foreach (var setting in _tridentSetting)
            {
                setting.VertexList = Intializate(SizeMap(setting.VerticalMode), setting.Direction);
            }
        }
        private int SizeMap(bool vertical)
        {
            if (vertical)
                return _sizeMap.x / 2;
            else
                return _sizeMap.y / 2;
        }
        private List<IVertex> Intializate(int sizeAxis, Vector3Int maskDirection)
        {
            List<IVertex> list = new List<IVertex>();
            for (int i = -sizeAxis; i < sizeAxis; i++)
            {
                var position = _tileMap.GetCellCenterWorld(maskDirection * i);
                if (_setting.isDebug)
                    InstatePoint(position);
                list.Add(new VertexTrident(position));
            }
            return list;
        }
        private void InstatePoint(Vector3 position)
        {
            var point = Instantiate(_setting.point, position, Quaternion.identity);
            point.transform.SetParent(transform);
        }
        private IEnumerator RunMode()
        {
            float progress = 0;
            GameObject lostTrident = null;
            while (progress <= 1f)
            {
                foreach (var setting in _tridentSetting)
                {
                    var point = ChooseVertex(setting.VertexList);
                    if (point != null)
                    {
                        lostTrident = point.InstateObject(_spawner, transform.parent);
                        lostTrident.transform.rotation = Quaternion.Euler(Vector3.forward * setting.MarkerAngle);
                        if (lostTrident.TryGetComponent<Marker>(out Marker marker))
                        {
                            marker.Constructor(setting.Angls, _warningTime, setting.Direction * _offset, _trident);
                        }
                    }
                }
                yield return new WaitForSeconds(_delay);
                progress += _delay / _duration;
            }
            yield return new WaitWhile(() => lostTrident != null);
            Destroy(gameObject);
        }

        private IVertex ChooseVertex(List<IVertex> list)
        {
            List<IVertex> unBusy = new List<IVertex>(); 
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].state == VertexState.UnBusy)
                    unBusy.Add(list[i]);
            }
            if (unBusy.Count == 0)
                return null;
            int index = Random.Range(0, unBusy.Count);
            return unBusy[index];
        }

    }
}