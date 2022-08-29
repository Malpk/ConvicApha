using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class PoolItem
{
    [Header("Spawn Setting")]
    [SerializeField] private int _maxCount;
    [Min(0)]
    [SerializeField] private int _distanceFormPlayer;
    [SerializeField] private bool _isInfinity;
    [Min(1f)]
    [SerializeField] private float _spawnProbility = 1f;
    [Header("Referemce")]
    [AssetReferenceUILabelRestriction("device")]
    [SerializeField] private AssetReferenceGameObject _perfabAsset;

    private SmartItem _perfab;

    private List<SmartItem> _poolActive = new List<SmartItem>();
    private List<SmartItem> _poolDeactive = new List<SmartItem>();

    public int DistanceFromPlayer => _distanceFormPlayer;
    public bool IsAcces => _poolActive.Count < _maxCount || _isInfinity;
    public float SpawnProbility => GetProbility();

    public async Task LoadAsset()
    {
        var load = _perfabAsset.LoadAssetAsync().Task;
        await load;
        try
        {
            if (!load.Result.TryGetComponent(out SmartItem item))
                throw new System.NullReferenceException("GameObject is not SpawnItem component");
            _perfab = item;
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
    }
    public void UnLaodAsset()
    {
        _perfab = null;
        _perfabAsset.ReleaseAsset();
    }
    public void ResetItem()
    {
        while (_poolActive.Count > 0)
        {
            var item = _poolActive[0];
            _poolActive.Remove(item);
            item.HideItem();
        }
    }
    public bool Create(out SmartItem item)
    {
        CheakState();
        if (IsAcces)
        {
            item = Instantiate();
            item.StartCoroutine(AddInPool(item));
            return item;
        }
        else
        {
            item = null;
            return false;
        }
    }

    private SmartItem Instantiate()
    {
        if (_poolDeactive.Count > 0)
        {
            var item = _poolDeactive[0];
            _poolDeactive.Remove(item);
            return item;
        }
        return MonoBehaviour.Instantiate(_perfab.gameObject).GetComponent<SmartItem>();
    }
    private void CheakState()
    {
        for (int i = 0; i < _poolActive.Count; i++)
        {
            if (!_poolActive[i].IsShow)
            {
                var item = _poolActive[i];
                _poolActive.Remove(item);
                if (_poolActive.Count > _poolDeactive.Count)
                    _poolDeactive.Add(item);
                else
                    MonoBehaviour.Destroy(item.gameObject);
            }
        }
        while (_poolActive.Count < _poolDeactive.Count)
        {
            var item = _poolDeactive[0];
            _poolDeactive.Remove(item);
            MonoBehaviour.Destroy(item.gameObject);
        }
    }
    private IEnumerator AddInPool(SmartItem item)
    {
        yield return new WaitWhile(() => !item.IsShow);
        _poolActive.Add(item);
    }
    private float GetProbility()
    {
        if (IsAcces)
            return _poolActive.Count > 0 ? _spawnProbility / _poolActive.Count : _spawnProbility;
        else
            return 0f;
    }
}
