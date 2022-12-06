using UnityEngine;
using MainMode.Items;

namespace MainMode.GameInteface
{
    public class HUDUI : UserInterface
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private HealthUI _healthUI;
        [SerializeField] private InventroryUI _inventoryUI;
        [SerializeField] private ReloadCell _abilityCell;
        [SerializeField] private SwitchScreenEffectUI _screenSwitcher;

        private void OnEnable()
        {
            ShowAction += () => _canvas.enabled = true;
            HideAction += () => _canvas.enabled = false;
        }

        private void OnDisable()
        {
            ShowAction -= () => _canvas.enabled = true;
            HideAction -= () => _canvas.enabled = false;
        }
        #region Health
        public void SetHealthPoint(int health)
        {
            _healthUI.SetupHelth(health);
        }
        public void UpdateHealth(int health)
        {
            _healthUI.Display(health);
        }
        #endregion
        #region Invetrory
        public void DisplayArtifact(Artifact artifact)
        {
            _inventoryUI.DisplayArtifact(artifact);
        }
        public void UpdateReloadArtifact(float progress)
        {
            _inventoryUI.DisplayReloadTime(progress);
        }
        public void UpdateArtifactState(bool state)
        {
            _inventoryUI.UpdateStateArtefact(state);
        }
        public void DisplayConsumableItem(ConsumablesItem item)
        {
            _inventoryUI.DisplayConsumablesItem(item);
        }
        #endregion
        #region ScreenEffect 
        public void ShowScreenEffect(EffectType effect, bool mode)
        {
            if(mode)
                _screenSwitcher.Show(effect);
            else
                _screenSwitcher.Hide(effect);
        }
        #endregion
        public void UpdateAbilityIcon(Sprite sprite, bool handAbillity = true)
        {
            _abilityCell.Intializate(sprite, handAbillity);
        }
        public void UpdateStateAbillity(bool mode)
        {
            _abilityCell.SetState(mode);
        }
        public void UpdateRelodingAbillity(float second)
        {
            _abilityCell.UpdateTime(second);
        }
    }
}