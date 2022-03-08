using Zenject;
using UnityEngine;
using TMPro;
using SwitchModeComponent;

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
            var typeMode = ModeType.None;
            if (_modeSwich.curreqSequence.TryGetComponent<UnderworldSequnce>(out UnderworldSequnce sequence))
                typeMode = sequence.curretTypeMode;
            _headDeadMenu.text = _messange.GetMessgange(typeMode);
        }
    }
}