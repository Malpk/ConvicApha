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
        [Header("Instantiate Object")]
        [SerializeField] private Shield _shield;
        [SerializeField] private VenomMine _gasMine;
        [SerializeField] private ToolRepairs[] _tools;
        [Header("Events")]
        [SerializeField] private UnityEvent _win;

        private int _countRepairShield;
        private List<Shield> _poolShield = new List<Shield>();
        private List<VenomMine> _poolVenomMine = new List<VenomMine>();

        private void Start()
        {
            if (_onStart)
            {
                InstateTools();
                InstanteShield(_shield);
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
        private void InstanteShield(Shield shield)
        {
            _countRepairShield = Random.Range(_countRangeShield.x, _countRangeShield.y);
            for (int i = 0; i < _countRepairShield; i++)
            {
                _poolShield.Add(BindShield(
                    InstantiateObject(shield.gameObject).GetComponent<Shield>()));
            }
        }
        private void InstateTools()
        {
            foreach (var tool in _tools)
            {
                InstantiateObject(tool.gameObject);
            }
        }
        private Shield BindShield(Shield shield)
        {
            shield.Intializate(_changeTest, _tools.Length);
            shield.RepairShieldAction += UpdateQuest;
            return shield;
        }
        private void InstanteVenimMine(VenomMine mine)
        {
            var count = Random.Range(_countRangeMine.x, _countRangeMine.y);
            for (int i = 0; i < count; i++)
            {
                _poolVenomMine.Add(
                    InstantiateObject(mine.gameObject).GetComponent<VenomMine>());
            }
        }

        private GameObject InstantiateObject(GameObject instantiateObject)
        {
            var freePoints = _map.GetFreePoints();
            var instantiate = Instantiate(instantiateObject, transform);
            freePoints[Random.Range(0, freePoints.Count)].SetObject(instantiate);
            return instantiate;
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