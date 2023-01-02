using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [System.Serializable]
    public class PoolDevice
    {
        [SerializeField] private PoolItem[] _pools;

        public void ClearPool()
        {
            foreach (var pool in _pools)
            {
                pool.ClearPool(true);
            }
        }

        public void Update()
        {
            foreach (var pool in _pools)
            {
                pool.Update();
            }
        }

        public bool GetPool(out PoolItem pool)
        {
            pool = null;
            var pools = GeneralProbility(out float amount);
            var choosee = Random.Range(0, 1.000f);
            if (pools.Count > 1)
            {
                var curretProbillity = 0f;
                for (int i = 0; i < pools.Count; i++)
                {
                    curretProbillity += pools[i].SpawnProbility / amount;
                    if (choosee <= curretProbillity)
                    {
                        pool = pools[i];
                        return true;
                    }
                }
            }
            else if (pools.Count > 0)
            {
                pool = pools[0];
                return true;
            }
            return false;
        }

        public List<PoolItem> GeneralProbility(out float amount)
        {
            amount = 0f;
            var list = new List<PoolItem>();
            for (int i = 0; i < _pools.Length; i++)
            {
                if (_pools[i].SpawnProbility > 0)
                {
                    amount += _pools[i].SpawnProbility;
                    list.Add(_pools[i]);
                }
            }
            return list;
        }
    }
}