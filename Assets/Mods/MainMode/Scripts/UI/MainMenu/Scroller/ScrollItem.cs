using UnityEngine;

namespace MainMode.GameInteface
{
    [CreateAssetMenu(menuName = "UIConfigs/ItenConfig")]
    public class ScrollItem : ScriptableObject
    {
        [SerializeField] private Sprite _itemImage;
        [SerializeField] private GameObject _perfab;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _desctiption;

        protected GameObject asset;

        public Sprite ItemImage => _itemImage;
        public string Name => _name;
        public Sprite Description => _desctiption;

        public T Create<T>() where T : MonoBehaviour
        {
            if (asset)
            {
                asset.SetActive(true);
                return asset.GetComponent<T>();
            }
            asset = Instantiate(_perfab);
            return asset.GetComponent<T>();
        }

        public void Delete()
        {
            asset.SetActive(false);
        }

    }
}
