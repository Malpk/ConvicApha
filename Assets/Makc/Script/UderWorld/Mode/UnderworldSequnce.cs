using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwitchModeComponent;

namespace Underworld
{
    public class UnderworldSequnce : MonoBehaviour,IModeForSwitch
    {
        [SerializeField] private float _delay; 
        [SerializeField] private List<Mode> _mods = new List<Mode>();

        private GameObject _mode;
        private SwitchMode _switchMode;
        private Coroutine _coroutine = null;

        public bool IsActive => _mode != null;
        public ModeType curretTypeMode { get; private set; }

        public void Constructor(SwitchMode swictMode)
        {
            if (_coroutine != null)
                return;
            _switchMode = swictMode;
            _coroutine = StartCoroutine(RunMode());
        }

        private IEnumerator RunMode()
        {
            foreach (var mode in _mods)
            {
                curretTypeMode = mode.type;
                _mode =  Instantiate(mode.mode, Vector3.zero, Quaternion.identity);
                _mode.transform.parent = transform;
                var mods = _mode.GetComponents<IModeForSwitch>();
                for (int i = 0; i < mods.Length; i++)
                {
                    mods[i].Constructor(_switchMode);
                }
                yield return new WaitWhile(() => (_mode != null));
                yield return new WaitForSeconds(GetDelay(mode.delay));
            }
            yield return new WaitForSeconds(_delay);
            Destroy(gameObject);
        }

        private float GetDelay(float modeDelay)
        {
            if (modeDelay > 0)
                return modeDelay;
            else
                return _delay;
        }
    }
}