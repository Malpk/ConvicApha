using UnityEngine;

public class DeviceHealSet : MonoBehaviour
{
    [Min(1)]
    [SerializeField] private int _health = 1;

    public void SetHealth(Player player)
    {
        if(player)
            player.Heal(_health);
    }
}
