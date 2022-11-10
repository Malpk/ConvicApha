using MainMode.GameInteface;
using UnityEngine;

namespace MainMode
{
    public  abstract class Receiver : MonoBehaviour
    {
        public abstract TypeDisplay DisplayType { get; }
    }
}
