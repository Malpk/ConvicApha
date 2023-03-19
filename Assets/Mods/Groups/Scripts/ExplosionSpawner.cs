using UnityEngine;

namespace MainMode
{
    public class ExplosionSpawner : MonoBehaviour
    {
        [SerializeField] private Pool _pool;
        [SerializeField] private FireWave _explosionPrefab;

        public void SpawnExlosion()
        {
            var explosion = _pool.Create(_explosionPrefab);
            explosion.GetComponent<FireWave>().SetMode(true);
            explosion.transform.position = transform.position;
            explosion.transform.rotation = transform.rotation;
        }
    }
}