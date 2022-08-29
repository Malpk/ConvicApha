using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Mode1921
{
    [RequireComponent(typeof(Animator), typeof(BoxCollider2D))]
    public class Shield : SmartItem, IItemInteractive
    {
        [Header("Requred Reference")]
        [Min(0)]
        [SerializeField] private int _countGames = 3;
        [Header("Requred Reference")]
        [SerializeField] private SpriteRenderer _display;

        private int _curretCount;
        private ToolSet _curretToolSet;
        private Animator _animator;
        private Collider2D _collider;
        private ChangeTest _changeTest;
        private IBlock[] _blockElements;


        public delegate void Action(Shield parent);
        public event Action RepairShieldAction;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
            _curretCount = _countGames;
            _collider.isTrigger = true;
            SetMode(false);
        }
        private void OnEnable()
        {
            ShowItemAction += () => SetMode(true);
            HideItemAction += () => SetMode(false);
        }
        private void OnDisable()
        {
            ShowItemAction -= () => SetMode(true);
            HideItemAction -= () => SetMode(false);
        }

        private void Start()
        {
            HideUI();
        }
        public bool Intializate(ChangeTest changeTest)
        {
            if (_changeTest == null)
            {
                _changeTest = changeTest;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Restart()
        {
            _curretCount = _countGames;
        }
        #region Repair
        public bool Interactive(Player player)
        {
            if (!player.TryGetComponent(out ToolSet toolSet) || _changeTest.IsRepairShield)
                return false;
            if (toolSet.IsAccessTools)
            {
                BlockPlayer(player);
                _curretToolSet = toolSet;
                if (player.TryGetComponent(out OxyGenSet oxyGen))
                {
                    _changeTest.RunGame(oxyGen, _curretCount);
                    _changeTest.CompliteGame += OnRepairShield;
                }
                else
                {
                    OnRepairShield(_curretCount);
                }
                return true;
            }
            toolSet.ShowHint();
            return false;
        }
        private void OnRepairShield(int compliteRepair)
        {
            _changeTest.CompliteGame -= OnRepairShield;
            _curretCount -= compliteRepair;
            UnBlcokPlayer();
            if (_curretCount <= 0)
            {
                HideUI();
                SetMode(false);
                if(_curretToolSet)
                    _curretToolSet.UseTool();
                if (RepairShieldAction != null)
                    RepairShieldAction(this);
            }
            _curretToolSet = null;
        }
        #endregion
        #region Blocking
        private void BlockPlayer(Player player)
        {
            _blockElements = player.GetComponents<IBlock>();
            if (_blockElements != null)
            {
                foreach (var element in _blockElements)
                {
                    element.Block();
                }
            }
        }
        private void UnBlcokPlayer()
        {
            if (_blockElements != null)
            {
                foreach (var element in _blockElements)
                {
                    element.UnBlock();
                }
                _blockElements = null;
            }
        }
        #endregion
        #region Display Items
        protected void SetMode(bool mode)
        {
            _curretCount = mode ? _countGames : 0; 
            _collider.enabled = mode;
            _animator.SetBool("Mode", !mode);
        }
        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out Player player))
            {
                _display.enabled = true;
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
            _display.enabled = false;
        }
    }
}