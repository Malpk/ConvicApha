using UnityEngine;
using MainMode.Items;

namespace MainMode.GameInteface
{
    public class InventroryUI : MonoBehaviour
    {
        [SerializeField] private InventoryView _item;
        [SerializeField] private ReloadCell _artifact;

        public void DisplayConsumablesItem(ConsumablesItem items)
        {
            if (items)
                _item.Display(items.Sprite, items.Count);
            else
                _item.Display(null, 0);
        }
        public void DisplayArtifact(Artifact artifact)
        {
            if (artifact)
                _artifact.Intializate(artifact.Sprite);
            else
                _artifact.Intializate(null);
        }
        public void DisplayReloadTime(float progress)
        {
            _artifact.UpdateTime(progress);
        }
        public void UpdateStateArtefact(bool state)
        {
            _artifact.SetState(state);
        }
    }
}