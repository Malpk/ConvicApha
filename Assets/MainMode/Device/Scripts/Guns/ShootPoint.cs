using UnityEngine;

namespace MainMode
{
    public class ShootPoint : MonoBehaviour
    {
        public delegate void Action();
        public event Action FireAction;

        private void Fire()
        {
            if (FireAction != null)
                FireAction();
        }
    }
}