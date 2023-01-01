using UnityEngine;

namespace MainMode.Room
{
    public abstract class RoomBehaviour : MonoBehaviour
    {
        public abstract bool IsPlay { get; }

        public abstract void Play();
        public abstract void Stop();
    }
}
