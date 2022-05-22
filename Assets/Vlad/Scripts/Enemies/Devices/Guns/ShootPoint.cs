using UnityEngine;

namespace BaseMode
{
    public class ShootPoint : MonoBehaviour
    {
        public delegate void Fire();
        public event Fire FireAction;

        private void Shoot()
        {
            if (FireAction != null)
                FireAction();
        }
    }
}