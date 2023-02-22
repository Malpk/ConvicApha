using System.Collections;
using UnityEngine;

public class SpawnerGroupSU : MonoBehaviour
{
    [SerializeField] private GameObject groupSU;
    [SerializeField] private Vector2[] SpawnPoints;
    private int pointsIndex;

    private void Start()
    {
        pointsIndex = Random.Range(0, SpawnPoints.Length);
        Instantiate(groupSU, SpawnPoints[pointsIndex], transform.rotation);
        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        float _lifeTime = BorderSpawn.bordersSpawnLifeTime;
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }
}
