using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using Zenject;

namespace Trident
{
    public class TridentMode : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Vector2Int _sizeMap;
        [SerializeField] private ModeSetting _setting;
        [SerializeField] private GameObject _verticalSpawner;
        [SerializeField] private GameObject _horizontalSpawner;

        [Inject] private TridentSetting _trident;

        private List<IVertex> _horizontal = new List<IVertex>();
        private List<IVertex> _vertical = new List<IVertex>();

        private void Awake()
        {
            _horizontal = Intializate(_sizeMap.y / 2, Vector3Int.up);
            _vertical = Intializate(_sizeMap.x / 2, Vector3Int.right);
        }
        private List<IVertex> Intializate(int sizeAxis, Vector3Int maskDirection)
        {
            List<IVertex> list = new List<IVertex>();
            for (int i = -sizeAxis; i < sizeAxis; i++)
            {
                var position = _tilemap.GetCellCenterWorld(maskDirection * i);
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
        private void Start()
        {
            StartCoroutine(RunMode());
        }
        private IEnumerator RunMode()
        {
            float progress = 0;
            while (progress <= 1f)
            {
                var point = ChooseVertex(_horizontal);
                if (point != null)
                {
                    point.InstateObject(_horizontalSpawner);
                }
                point = ChooseVertex(_vertical);
                if (point != null)
                    point.InstateObject(_verticalSpawner);
                yield return new WaitForSeconds(_trident.startDelay);
                progress += _trident.startDelay / _setting.duration;
            }
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