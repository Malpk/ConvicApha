using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Map;
using UnityEngine.Events;

namespace MainMode.Mode1921
{
    public class Mode1921 : MonoBehaviour
    {
        [Header("General Seting")]
        [SerializeField] private bool _onStart = true;
        [SerializeField] private Vector2Int _countRangeMine;
        [SerializeField] private Vector2Int _countRangeShield;
        [Header("Requred Reference")]
        [SerializeField] private Shield _shield;
        [SerializeField] private MapGrid _map;
        [SerializeField] private VenomMine _gasMine;
        [Header("Events")]
        [SerializeField] private UnityEvent _win;

        private int _countRepairShield;
        private List<Shield> _poolShield = new List<Shield>();
        private List<VenomMine> _poolVenomMine = new List<VenomMine>();
        private void Start()
        {
            if (_onStart)
            {
                InstanteShield(_shield);
                InstanteVenimMine(_gasMine);
            }
        }

        private void InstanteShield(Shield shield)
        {
            _countRepairShield = Random.Range(_countRangeShield.x, _countRangeShield.y);
            for (int i = 0; i < _countRepairShield; i++)
            {
                var freePoints = _map.GetFreePoints();
                var instante = Instantiate(shield.gameObject, transform);
                freePoints[Random.Range(0, freePoints.Count)].SetObject(instante);
                _poolShield.Add(instante.GetComponent<Shield>());
                _poolShield[_poolShield.Count - 1].RepairShieldAction += UpdateQuest;
            }
        }
        private void InstanteVenimMine(VenomMine mine)
        {
            var count = Random.Range(_countRangeMine.x, _countRangeMine.y);
            for (int i = 0; i < count; i++)
            {
                var freePoints = _map.GetFreePoints();
                var instante = Instantiate(mine.gameObject, transform);
                freePoints[Random.Range(0, freePoints.Count)].SetObject(instante);
                _poolVenomMine.Add(instante.GetComponent<VenomMine>());
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