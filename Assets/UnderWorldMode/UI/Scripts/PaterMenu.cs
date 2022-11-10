using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MainMode.GameInteface;

namespace Underworld
{
    public class PaterMenu : UserInterface
    {
        [Header("Buttons")]
        [SerializeField] private PaternButton _perfab;
        [SerializeField] private PaternButtonInfo[] _buttonConfig;
        [Header("Reference")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private SwitchPatern _switcher;
        [SerializeField] private Transform _contentView;
        [SerializeField] private Button _startModeButton;
        [SerializeField] private Image _signalImage;

        private ModeType _chooseMode;
        private ISelect _selectButton;

        private Coroutine _startMode;
        private List<PaternButton> _buttons = new List<PaternButton>();

        public override UserInterfaceType Type => UserInterfaceType.Other;

        protected void Awake()
        {
            foreach (var config in _buttonConfig)
            {
                var button = Instantiate(_perfab.gameObject, _contentView).GetComponent<PaternButton>();
                button.SetMode(config.TypeMode);
                button.SetPaternIconc(config.ButtonIcon);
                button.SetButtonLable(config.ButtonLable);
                button.MessangeAction += SetMode;
                _buttons.Add(button);
            }
        }
        private void OnEnable()
        {
            HideAction += ResetMenu;
            ShowAction += () => _canvas.enabled = true;
            HideAction += () => _canvas.enabled = false;
            _startModeButton.onClick.AddListener(StartMode);
        }
        private void OnDisable()
        {
            HideAction -= ResetMenu;
            ShowAction -= () => _canvas.enabled = true;
            HideAction -= () => _canvas.enabled = false;
            _startModeButton.onClick.RemoveListener(StartMode);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (!IsShow)
                    Show();
                else
                    Hide();
            }
        }
        public void StartMode()
        {
            SetActivate(_chooseMode);
            _signalImage.color = Color.red;
            _startMode = StartCoroutine(WaitCompliteMode());
        }
        protected void SetMode(ISelect sender,ModeType mode)
        {
            _chooseMode = mode;
            if (_selectButton != null)
                _selectButton.UnSelect();
            sender.Select();
            _selectButton = sender;
            if(_startMode==null)
                _startModeButton.interactable = true;
        }
        private void SetActivate(ModeType type)
        {
            _switcher.ActivateMode(type);
            Hide();
        }
        private IEnumerator WaitCompliteMode()
        {
            yield return new WaitWhile(() => !_switcher.IsReady);
            _signalImage.color = Color.green;
            if (_selectButton != null)
                _startModeButton.interactable = true;
            _startMode = null;
        }
        private void ResetMenu()
        {
            _startModeButton.interactable = false;
            if (_selectButton != null)
            {
                _selectButton.UnSelect();
                _selectButton = null;
            }
        }
    }
}