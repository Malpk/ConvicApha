using UnityEngine;
using MainMode.Items;

namespace MainMode.GameInteface
{
    public class HUDInteface : UserInterface
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private HealthUI _healthUI;
        [SerializeField] private InventroryUI _inventoryUI;
        [SerializeField] private SwitchScreenEffectUI _screenSwitcher;

        public override UserInterfaceType Type => UserInterfaceType.HUD;

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
        public void SetHealth(int health)
        {
            _healthUI.Display(health);
        }
        #endregion
        #region Invetrory
        public void DisplayArtifact(Item item)
        {
            if (item)
            {
                if (!item.IsInfinity)
                    _inventoryUI.DisplayArtifact(item.Sprite, item.Count);
                else
                    _inventoryUI.DisplayInfinity(item.Sprite);
            }
            else
            {
                _inventoryUI.DisplayArtifact(null);
            }
        }
        public void DisplayConsumableItem(Item item)
        {
            if (item)
            {
                _inventoryUI.DisplayConsumablesItem(item.Sprite, item.Count);
            }
            else
            {
                _inventoryUI.DisplayConsumablesItem(null);
            }
        }
        #endregion
        #region ScreenEffect 
        public void ShowScreenEffect(EffectType effect)
        {
            _screenSwitcher.Show(effect);
        }
        public void HideScreenEffect(EffectType effect)
        {
            _screenSwitcher.Hide(effect);
        }
        #endregion
    }
}