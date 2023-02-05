using UnityEngine;

public class TriangleSpawnerSU : MonoBehaviour
{
    private GameObject groupSU;
    private Vector3[] trianglePoints;
    private int pointsIndex;
    private float LifeTime = 2f;

    private void Start()
    {
        pointsIndex = Random.Range(0, trianglePoints.Length);
        Instantiate(groupSU, trianglePoints[pointsIndex], transform.rotation);
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
