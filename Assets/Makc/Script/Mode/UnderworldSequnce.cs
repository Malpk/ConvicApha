using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwitchMode;

namespace Underworld
{
    public class UnderworldSequnce : MonoBehaviour,ISequence
    {
        [SerializeField] private float _delay; 
        [SerializeField] private List<Mode> _mods = new List<Mode>();

        private SwitchMods _switchMode;
        private Coroutine _coroutine = null;

        public ModeType curretTypeMode { get; private set; }

        public void Constructor(SwitchMods swictMode)
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
                var instateMode =  Instantiate(mode.mode, Vector3.zero, Quaternion.identity);
                instateMode.transform.parent = transform;
                var mods = instateMode.GetComponents<ISequence>();
                for (int i = 0; i < mods.Length; i++)
                {
                    mods[i].Constructor(_switchMode);
                }
                yield return new WaitWhile(() => (instateMode != null));
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