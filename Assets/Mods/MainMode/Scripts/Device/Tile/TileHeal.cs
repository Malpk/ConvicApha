using UnityEngine;

namespace MainMode
{
    public class TileHeal : TileDevice
    {
        [Min(1)]
        [SerializeField] private int _healValie = 1;
        public override TrapType DeviceType => TrapType.TileHeal;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                player.Heal(_healValie);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                Deactivate();
            }
        }
    }
}
