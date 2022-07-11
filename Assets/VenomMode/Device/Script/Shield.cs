using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Mode1921
{
    [RequireComponent(typeof(Animator), typeof(BoxCollider2D))]
    public class Shield : MonoBehaviour, IItemInteractive
    {
        [Header("General Setting")]
        [Min(0)]
        [SerializeField] private int _countTest = 3;
        [Header("Requred Reference")]
        [SerializeField] private Canvas _canvas;

        private Animator _animator;
        private Collider2D _collider;
        private ChangeTest _changeTest;

        public delegate void Action(Shield parent);
        public event Action RepairShieldAction;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
            _collider.isTrigger = true;
        }
        private void Start()
        {
            HideUI();
        }
        public bool Intializate(ChangeTest changeTest, int countTest)
        {
            if (_changeTest == null)
            {
                _changeTest = changeTest;
                _countTest = countTest;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Interactive(Player player)
        {
            if (!player.TryGetComponent(out ToolSet set))
                return false;
            if(set.CountTools >= _countTest)
            {
                if (player.TryGetComponent(out OxyGenSet oxyGen) && _changeTest != null)
                {
                    oxyGen.Pause();
                    _changeTest.RunGame(oxyGen, _countTest);
                    _changeTest.CompliteGame += OnRepairShield;
                }
                else
                {
                    OnRepairShield(_countTest);
                }
                return true;
            }
            set.ShowHint();
            return false;
        }

        private void OnRepairShield(int compliteRepair)
        {
            HideUI();
            _changeTest.CompliteGame -= OnRepairShield;
            _countTest = Mathf.Clamp(_countTest - compliteRepair, 0, _countTest);
            if (_countTest <= 0)
            {
                SetMode(false);
                _animator.SetBool("Mode", true);
            }
        }

        private void SetMode(bool mode)
        {
            enabled = mode;
            _collider.enabled = mode;
            if (RepairShieldAction != null && mode)
                RepairShieldAction(this);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out Player player))
            {
                _canvas.enabled = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                HideUI();
            }
        }

        private void HideUI()
        {
            _canvas.enabled = false;
        }
    }
}