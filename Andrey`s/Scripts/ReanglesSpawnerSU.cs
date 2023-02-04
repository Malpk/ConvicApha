using UnityEngine;

public class ReanglesSpawnerSU : MonoBehaviour
{
    private GameObject groupSU;
    private Vector3[] reanglesPoints;
    private int pointsIndex;
    private float LifeTime = 2f;

    private void Start()
    {
        pointsIndex = Random.Range(0, reanglesPoints.Length);
        Instantiate(groupSU, reanglesPoints[pointsIndex], transform.rotation);
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
