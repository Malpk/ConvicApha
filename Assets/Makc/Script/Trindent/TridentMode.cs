using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using SwitchMode;

namespace Trident 
{
    public class TridentMode : MonoBehaviour,ISequence
    {
        [SerializeField] private Vector2Int _sizeMap;
        [SerializeField] private Mode _setting;
        [SerializeField] private GameObject _verticalSpawner;
        [SerializeField] private GameObject _horizontalSpawner;

        private Tilemap _tileMap;
        private TridentSetting _trident;
        private Coroutine _coroutine;

        private List<IVertex> _horizontal = new List<IVertex>();
        private List<IVertex> _vertical = new List<IVertex>();


        public void Constructor(SwitchMods swictMode)
        {
            if(_coroutine != null)
                return;
            _tileMap = swictMode.tileMap;
            _trident = swictMode.trident;
            Intializate();
            _coroutine = StartCoroutine(RunMode());
        }

        private void Intializate()
        {
            _horizontal = Intializate(_sizeMap.y / 2, Vector3Int.up);
            _vertical = Intializate(_sizeMap.x / 2, Vector3Int.right);
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
            while (progress <= 1f)
            {
                var point = ChooseVertex(_horizontal);
                if (point != null)
                {
                    point.InstateObject(_horizontalSpawner,transform.parent);
                }
                point = ChooseVertex(_vertical);
                if (point != null)
                    point.InstateObject(_verticalSpawner,transform);
                yield return new WaitForSeconds(_trident.startDelay);
                progress += _trident.startDelay / _setting.duration;
            }
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