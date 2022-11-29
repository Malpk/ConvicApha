using UnityEngine;
using PlayerComponent;
using MainMode.GameInteface;

public class PlayerUIBinder : MonoBehaviour
{
    [SerializeField] private HUDUI _hud;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private PlayerEffectSet _effectSet;

    private void OnEnable()
    {
        _player.OnSetupMaxHealth += _hud.SetHealthPoint;
        _player.OnUpdateHealth += _hud.SetHealth;
        _effectSet.OnUpdateScreen += _hud.ShowScreenEffect;
        _inventory.OnUpdateArtefacct += _hud.DisplayArtifact;
        _inventory.OnUpdateConsumableItem += _hud.DisplayConsumableItem;
    }

    private void OnDisable()
    {
        _player.OnSetupMaxHealth -= _hud.SetHealthPoint;
        _player.OnUpdateHealth -= _hud.SetHealth;
        _inventory.OnUpdateArtefacct -= _hud.DisplayArtifact;
        _inventory.OnUpdateConsumableItem -= _hud.DisplayConsumableItem;
        _effectSet.OnUpdateScreen -= _hud.ShowScreenEffect;
    }

}
