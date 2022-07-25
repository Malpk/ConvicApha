using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MainMode.GameInteface;

namespace MainMode.Mode1921
{
    public class Mode1921 : MonoBehaviour
    {
        [Header("General Seting")]
        [SerializeField] private bool _playOnStart = true;
        [SerializeField] private Vector2Int _countRangeMine;
        [Header("Requred Reference")]
        [SerializeField] private MapGrid _map;
        [SerializeField] private ChangeTest _changeTest;
        [Header("Spawn Setting")]
        [SerializeField] private float _minDistanceTools;
        [SerializeField] private float _minDistanceShield;
        [Header("Instantiate Object")]
        [SerializeField] private Shield _shield;
        [SerializeField] private VenomMine _gasMine;
        [SerializeField] private ToolRepairs[] _tools;
        [Header("Events")]
        [SerializeField] public UnityEvent Win;

        private int _countRepairShield;
        private List<IMapItem> _poolShield = new List<IMapItem>();
        private List<IMapItem> _poolMine = new List<IMapItem>();
        private List<IMapItem> _poolTool = new List<IMapItem>();


        private void Start()
        {
            if(_playOnStart)
                CreateMap();
        }
        public void Intializate(ChangeTest test)
        {
            _changeTest = test;
            CreateMap();
        }
        private void UpdateQuest(Shield parent)
        {
            _countRepairShield--;
            if (_countRepairShield <= 0)
            {
                Win.Invoke();
            }
        }

        #region Create Map
        public void CreateMap()
        {
            _countRepairShield = _tools.Length;
            if (_poolShield.Count == 0)
            {
                CreateShield(_shield, _tools.Length);
            }
            var count = Random.Range(_countRangeMine.x, _countRangeMine.y);
            if(count < _poolMine.Count)
                DeleteItem(_poolMine, _poolMine.Count - count);
            else
                AddVenomMine(_gasMine, count - _poolMine.Count);
            if(_poolTool.Count == 0)
                CreateTools(_tools);
            SetItemPosition();
        }
        private void SetItemPosition()
        {
            SetPosition(_poolShield, _minDistanceShield);
            SetPosition(_poolTool, _minDistanceTools);
            SetPosition(_poolMine);
        }
        private void CreateShield(Shield perfab, int count)
        {
            count = count < 0 ? 0 : count;
            for (int i = 0; i < count; i++)
            {
                var item = Instantiate(perfab.gameObject, transform);
                if (item.TryGetComponent(out Shield shield))
                {
                    _poolShield.Add(shield);
                    shield.Intializate(_changeTest);
                    BindShield(shield);
                }
            }
        }
        private void AddVenomMine(VenomMine perfab, int count)
        {
            count = count < 0 ? 0 : count;
            for (int i = 0; i < count; i++)
            {
                _poolMine.Add(Instantiate(perfab, transform).GetComponent<IMapItem>());
            }
        }
        private void CreateTools(ToolRepairs[] toolsPerfabs)
        {
            for (int i = 0; i < toolsPerfabs.Length; i++)
            {
                _poolTool.Add(Instantiate(toolsPerfabs[i].gameObject, transform).GetComponent<IMapItem>());
            }
        }
        private void DeleteItem(List<IMapItem> pool, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var item = pool[0];
                pool.Remove(pool[0]);
                item.Delete();
            }
        }
        private Shield BindShield(Shield shield)
        {
            shield.Intializate(_changeTest);
            shield.RepairShieldAction += UpdateQuest;
            return shield;
        }
        #endregion
        #region SetPosition
        private void SetPosition(List<IMapItem> items,float distance = 0)
        {
            var points = GetPoints(items.Count, distance);
            for (int i = 0; i < points.Count; i++)
            {
                points[i].SetItem(items[i]);
                items[i].SetMode(true);
            }
        }
        #endregion
        #region GetPoints
        private List<Point> GetPoints(int count ,float distance = 0)
        {
            var busyPoints = new List<Point>();
            for (int i = 0; i < count; i++)
            {
                var point = GetPoint(busyPoints, distance);
                if (point != null)
                    busyPoints.Add(point);
                else
                    break;
            }
            return busyPoints;
        }
        private Point GetPoint(List<Point> busyPoints, float distance)
        {
            var list = new List<Point>();
            var map = _map.GetFreePoints();
            for (int i = 0; i < map.Count; i++)
            {
                if (CheakDistance(map[i].Position, busyPoints, distance))
                    list.Add(map[i]);
            }
            return list.Count > 0 ? list[Random.Range(0, list.Count)] : null;
        }
        private bool CheakDistance(Vector2 position, List<Point> busyPoints, float distance)
        {
            foreach (var item in busyPoints)
            {
                if (Vector2.Distance(item.Position, position) < distance)
                    return false;
            }
            return true;
        }
        #endregion
    }
}