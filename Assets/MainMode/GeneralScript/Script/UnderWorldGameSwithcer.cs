using UnityEngine;
using MainMode;

namespace Underworld
{
    public class UnderWorldGameSwithcer : GameSwitcher
    {
        [SerializeField] private TImerCount _timeCounter;
        [SerializeField] private UnderWorldGameBuilder _builder;

        protected override void PlayMessange()
        {
            _builder.Play();
            _timeCounter.Play();
        }

        protected override void StopMessange()
        {
            _builder.Stop();
            _timeCounter.Stop();
        }
    }
}