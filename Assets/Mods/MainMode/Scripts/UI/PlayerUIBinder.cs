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
        _player.OnUpdateHealth += _hud.SetHealth;
        _player.OnSetupMaxHealth += _hud.SetHealthPoint;
        _effectSet.OnUpdateScreen += _hud.ShowScreenEffect;
        _inventory.OnUpdateConsumableItem += _hud.DisplayConsumableItem;
        _inventory.OnUpdateArtefact += _hud.DisplayArtifact;
        _inventory.OnUpdateStateArtefact += _hud.UpdateArtifactState;
        _inventory.OnUpdateReloadArtefact += _hud.UpdateReloadArtifact;
    }

    private void OnDisable()
    {
        _player.OnUpdateHealth -= _hud.SetHealth;
        _player.OnSetupMaxHealth -= _hud.SetHealthPoint;
        _effectSet.OnUpdateScreen -= _hud.ShowScreenEffect;
        _inventory.OnUpdateConsumableItem -= _hud.DisplayConsumableItem;
        _inventory.OnUpdateArtefact -= _hud.DisplayArtifact;
        _inventory.OnUpdateStateArtefact -= _hud.UpdateArtifactState;
        _inventory.OnUpdateReloadArtefact -= _hud.UpdateReloadArtifact;
    }

}
