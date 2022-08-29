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

        private List<SmartItem> _poolShield = new List<SmartItem>();
        private List<SmartItem> _poolMine = new List<SmartItem>();
        private List<SmartItem> _poolTool = new List<SmartItem>();


        private void Start()
        {
            if (_playOnStart)
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
            if (count < _poolMine.Count)
                DeleteItem(_poolMine, _poolMine.Count - count);
            else
                AddVenomMine(_gasMine, count - _poolMine.Count);
            if (_poolTool.Count == 0)
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
                    shield.ShowItem();
                    BindShield(shield);
                }
            }
        }
        private void AddVenomMine(VenomMine perfab, int count)
        {
            count = count < 0 ? 0 : count;
            for (int i = 0; i < count; i++)
            {
                var mine = Instantiate(perfab, transform).GetComponent<SmartItem>();
                mine.ShowItem();
                _poolMine.Add(mine);
            }
        }
        private void CreateTools(ToolRepairs[] toolsPerfabs)
        {
            for (int i = 0; i < toolsPerfabs.Length; i++)
            {
                var tool = Instantiate(toolsPerfabs[i].gameObject, transform).GetComponent<SmartItem>();
                tool.ShowItem();
                _poolTool.Add(tool);
            }
        }
        private void DeleteItem(List<SmartItem> pool, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var item = pool[0];
                pool.Remove(pool[0]);
                item.HideItem();
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
        private void SetPosition(List<SmartItem> items, float distance = 0)
        {
            var points = GetPoints(items.Count, distance);
            for (int i = 0; i < points.Count; i++)
            {
                var point = points[i];
                var item = items[i];
                point.SetItem(item);
                item.HideItemAction += () => ReturnPoints(item, point);
            }
        }
        #endregion
        #region GetPoints
        private List<Point> GetPoints(int count, float distance = 1)
        {
            var choosePoints = new List<Point>();
            for (int i = 0; i < count; i++)
            {
                var point = GetPoint(choosePoints, distance);
                if (point != null)
                {
                    choosePoints.Add(point);
                    _map.Points.Remove(point);
                }
                else
                {
                    break;
                }
            }
            return choosePoints;
        }
        private Point GetPoint(List<Point> busyPoints, float distance)
        {
            var list = new List<Point>();
            var map = _map.Points;
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
        private void ReturnPoints(SmartItem item ,Point point)
        {
            item.HideItemAction -= () => ReturnPoints(item, point);
            _map.Points.Add(point);
        }
        #endregion
    }
}