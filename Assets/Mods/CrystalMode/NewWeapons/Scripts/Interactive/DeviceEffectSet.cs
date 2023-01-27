using UnityEngine;
using PlayerComponent;

namespace MainMode
{
    public class DeviceEffectSet : MonoBehaviour
    {
        [SerializeField] private float _timeActiveEffect;
        [SerializeField] private MovementEffect _effect;

        public void SetEffect(Player player)
        {
            if(player)
                player.AddEffect(_effect, _timeActiveEffect);
        }
    }
}
