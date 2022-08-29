using UnityEngine;
using UnityEngine.Events;

namespace Underworld
{
    [System.Serializable]
    public class PaternButtonInfo 
    {
        [SerializeField] private string _buttonName;
        [SerializeField] private ModeType _type;
        [SerializeField] private Sprite _buttonIcon;

        public string ButtonLable => _buttonName;
        public Sprite ButtonIcon => _buttonIcon;
        public ModeType TypeMode => _type;
    }
}