using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace Underworld
{
    public class PaternButton : MonoBehaviour, ISelect
    {
        [SerializeField] private Image _paternIcon;
        [SerializeField] private Image _selectImage;
        [SerializeField] private Button _paternNutton;
        [SerializeField] private TextMeshProUGUI _buttonLable;

        private ModeType _type;

        public delegate void Messange(ISelect sender,ModeType type);
        public event Messange MessangeAction;

        private void OnEnable()
        {
            _paternNutton.onClick.AddListener(SendMessange);
        }
        private void OnDisable()
        {
            _paternNutton.onClick.RemoveListener(SendMessange);
        }
        public void SetPaternIconc(Sprite sprite)
        {
            _paternIcon.sprite = sprite;
        }
        public void SetButtonLable(string lable)
        {
            _buttonLable.text = lable;
        }
        public void SetMode(ModeType type)
        {
            _type = type;
        }
        public void Select()
        {
            _selectImage.enabled = true;
        }
        public void UnSelect()
        {
            _selectImage.enabled = false;
        }
        private void SendMessange()
        {
            if (MessangeAction != null)
                MessangeAction(this,_type);
        }
    }
}
