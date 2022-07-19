using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MainMode.Map;
using MainMode.GameInteface;

namespace MainMode.Mode1921
{
    public class Mode1921 : MonoBehaviour
    {
        [Header("General Seting")]
        [SerializeField] private bool _onStart = true;
        [SerializeField] private Vector2Int _countRangeMine;
        [SerializeField] private Vector2Int _countRangeShield;
        [Header("Requred Reference")]
        [SerializeField] private MapGrid _map;
        [SerializeField] private ChangeTest _changeTest;
        [Header("Spawn Setting")]
        [SerializeField] private float _minDistanceTools;
        [SerializeField] private float _miuDistanceShield;
        [Header("Instantiate Object")]
        [SerializeField] private Shield _shield;
        [SerializeField] private VenomMine _gasMine;
        [SerializeField] private ToolRepairs[] _tools;
        [Header("Events")]
        [SerializeField] private UnityEvent _win;

        private int _countRepairShield;
        private List<Shield> _poolShield = new List<Shield>();
        private FactoryGameObjecs1921 _factory;

        private void Awake()
        {
            _factory = new FactoryGameObjecs1921(_map);
        }

        private void Start()
        {
            if (_onStart)
            {
                InstateTools(_tools);
                InstanteShield(_shield, _miuDistanceShield);
                InstanteVenimMine(_gasMine);
            }
        }
        public bool Intializate(ChangeTest test)
        {
            if (_changeTest != null)
                return false;
            _changeTest = test;
            return true;
        }
        private void InstanteShield(Shield perfab, float distance)
        {
            _countRepairShield = Random.Range(_countRangeShield.x, _countRangeShield.y);
            var shields = _factory.Create(perfab.gameObject, _countRepairShield, distance);
            BindTransform(shields);
            foreach (var item in shields)
            {
                if (item.TryGetComponent(out Shield shield))
                {
                    _poolShield.Add(shield);
                    BindShield(shield);
                }
            }
        }
        private Shield BindShield(Shield shield)
        {
            shield.Intializate(_changeTest, _tools.Length);
            shield.RepairShieldAction += UpdateQuest;
            return shield;
        }
        private void InstateTools(ToolRepairs[] toolsPerfabs)
        {
            var tools = new GameObject[toolsPerfabs.Length];
            for (int i = 0; i < toolsPerfabs.Length; i++)
            {
                tools[i] = toolsPerfabs[i].gameObject;
            }
            BindTransform(_factory.Create(tools, _minDistanceTools));
        }

        private void InstanteVenimMine(VenomMine perfab)
        {
            var count = Random.Range(_countRangeMine.x, _countRangeMine.y);
            BindTransform(_factory.Create(perfab.gameObject, count));
        }
        private void BindTransform(List<GameObject> items)
        {
            foreach (var item in items)
            {
                item.transform.parent = transform;
            }
        }
        private void UpdateQuest(Shield parent)
        {
            _countRepairShield--;
            if (_countRepairShield <= 0)
            {
                _win.Invoke();
            }
        }
    }
}