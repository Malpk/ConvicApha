using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Trident;
using SwitchMode;

namespace Underworld
{
    public class AddtionMode : MonoBehaviour
    {
        [Header("Game Setting")]
        [SerializeField] private float _startDelay;
        [Header("Perfab Setting")]
        [SerializeField] private GameObject _tridentMode;

        [Inject] private SwitchMods _switchMods;
        [Inject] private CameraAnimation _cameraAnimation;

        private void OnEnable()
        {
            _cameraAnimation.CompliteAction += Run;
        }
        private void OnDisable()
        {
            _cameraAnimation.CompliteAction -= Run;
        }
        private void Run()
        {
            StartCoroutine(RunMode());
        }

        private IEnumerator RunMode()
        {
            yield return new WaitForSeconds(_startDelay);
            var  mode = Instantiate(_tridentMode, transform.position, Quaternion.identity);
            mode.transform.parent = transform;
            if (mode.TryGetComponent<TriggerTridentMode>(out TriggerTridentMode trident))
            {
                trident.Constructor(_switchMods);
                StartCoroutine(TrakingGameState(trident, _switchMods));
            }
            else
            {
                Debug.LogError("Object is not addition");
            }
            yield return null;
        }
        private IEnumerator TrakingGameState(TriggerTridentMode mode, SwitchMods switchMode)
        {
            while (true)
            {
                yield return new WaitWhile(() => !GetRight(switchMode));
                mode.ActiveteMode();
                yield return new WaitWhile(() => GetRight(switchMode));
                mode.DeactivateMode();
            }
        }
        private bool GetRight(SwitchMods switchMode)
        {
            if (switchMode.curreqSequence.TryGetComponent<UnderworldSequnce>(out UnderworldSequnce sequnce))
            {
                var state = CheakCurretMode(sequnce.curretTypeMode);
                return state && switchMode.isAttackMode;
            }
            else
            {
                return false;
            }
        }
        private bool CheakCurretMode(ModeType type)
        {
            switch (type)
            {
                case ModeType.Base:
                    return false;
                case ModeType.Trident:
                    return false;
                default:
                    return true;
            }
        }
    }
}