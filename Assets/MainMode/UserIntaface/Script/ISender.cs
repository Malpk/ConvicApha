using UnityEngine;
using MainMode.GameInteface;

namespace MainMode
{
    public interface ISender
    {
        public TypeDisplay TypeDisplay { get; }
        public bool AddReceiver(Receiver receiver);
    }
}