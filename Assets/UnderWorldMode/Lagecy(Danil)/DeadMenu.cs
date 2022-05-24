using Zenject;
using UnityEngine;
using TMPro;

namespace Underworld
{
    public class DeadMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _headDeadMenu;
        [SerializeField] private ModeMessange _messange;

        [Inject] private GameEvent _gameEvent;
        [Inject] private SwitchMode _modeSwich;

        private void OnEnable()
        {
            _gameEvent.DeadAction += EnterMessange;
        }
        private void OnDisable()
        {
            _gameEvent.DeadAction -= EnterMessange;
        }

        private void EnterMessange()
        {
            var typeMode = ModeTypeNew.BaseMode;
               // typeMode = _modeSwich.Type;
            _headDeadMenu.text = _messange.GetMessgange(typeMode);
        }
    }
}