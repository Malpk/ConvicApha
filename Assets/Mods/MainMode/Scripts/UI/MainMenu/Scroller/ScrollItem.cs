using UnityEngine;

namespace MainMode.GameInteface
{
    [CreateAssetMenu(menuName = "UIConfigs/ItenConfig")]
    public class ScrollItem : ScriptableObject
    {
        [SerializeField] private Sprite _itemImage;
        [SerializeField] private GameObject _perfab;
        [SerializeField] private string _name;
        [SerializeField] private string _desctiption;

        private GameObject _asset;

        public Sprite ItemImage => _itemImage;
        public string Name => _name;
        public string Description => _desctiption;

        public T Create<T>() where T : MonoBehaviour
        {
            if (_asset)
            {
                _asset.SetActive(true);
                return _asset.GetComponent<T>();
            }
            _asset = Instantiate(_perfab);
            return _asset.GetComponent<T>();
        }

        public void Delete()
        {
            _asset.SetActive(false);
        }

    }
}
