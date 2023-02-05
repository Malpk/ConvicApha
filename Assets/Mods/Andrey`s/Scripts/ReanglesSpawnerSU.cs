using UnityEngine;

public class ReanglesSpawnerSU : MonoBehaviour
{
    [SerializeField] private GameObject _groupSU;
    private Vector3[] reanglesPoints;
    private int pointsIndex;
    private float LifeTime = 2f;

    private void Start()
    {
        pointsIndex = Random.Range(0, reanglesPoints.Length);
        Instantiate(_groupSU, reanglesPoints[pointsIndex], transform.rotation);
    }

    private void Update()
    {
        LifeTime -= Time.deltaTime;

        if (LifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
