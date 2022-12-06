using UnityEngine;
using PlayerComponent;
using MainMode.GameInteface;

public class PlayerUIBinder : MonoBehaviour
{
    [SerializeField] private HUDUI _hud;
    [SerializeField] private PlayerBehaviour _player;
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private PlayerEffectSet _effectSet;
    [SerializeField] private PlayerBaseAbillitySet _abillity;

    private void Awake()
    {
        _effectSet.OnUpdateScreen += _hud.ShowScreenEffect;
        _inventory.OnUpdateConsumableItem += _hud.DisplayConsumableItem;
        _inventory.OnUpdateArtefact += _hud.DisplayArtifact;
        _inventory.OnUpdateStateArtefact += _hud.UpdateArtifactState;
        _inventory.OnUpdateReloadArtefact += _hud.UpdateReloadArtifact;
        if (_abillity)
            BindAbillityUI(_abillity);
        if (_player)
            BindHealthPlayerUI(_player);
    }
    public void BindAbillityUI(PlayerBaseAbillitySet abillity)
    {
        UnBindAbillityUI();
        abillity.OnUpdateState += _hud.UpdateStateAbillity;
        abillity.OnUpdateIcon += _hud.UpdateAbilityIcon;
        abillity.OnReloading += _hud.UpdateRelodingAbillity;
    }
    public void UnBindAbillityUI()
    {
        if (_abillity)
        {
            _abillity.OnUpdateState -= _hud.UpdateStateAbillity;
            _abillity.OnUpdateIcon -= _hud.UpdateAbilityIcon;
            _abillity.OnReloading -= _hud.UpdateRelodingAbillity;
        }
    }
    public void BindHealthPlayerUI(PlayerBehaviour player)
    {
        UnBindHealthPlayerUI();
        player.OnUpdateHealth.AddListener(_hud.UpdateHealth);
        player.OnSetupMaxHealth.AddListener(_hud.SetHealthPoint);
    }
    public void UnBindHealthPlayerUI()
    {
        if (_player)
        {
            _player.OnUpdateHealth.RemoveListener(_hud.UpdateHealth);
            _player.OnSetupMaxHealth.RemoveListener(_hud.SetHealthPoint);
        }
    }
    private void OnDestroy()
    {
        _effectSet.OnUpdateScreen -= _hud.ShowScreenEffect;
        _inventory.OnUpdateConsumableItem -= _hud.DisplayConsumableItem;
        _inventory.OnUpdateArtefact -= _hud.DisplayArtifact;
        _inventory.OnUpdateStateArtefact -= _hud.UpdateArtifactState;
        _inventory.OnUpdateReloadArtefact -= _hud.UpdateReloadArtifact;
        if (_abillity)
            UnBindAbillityUI();
        if (_player)
            UnBindHealthPlayerUI();
    }


}
