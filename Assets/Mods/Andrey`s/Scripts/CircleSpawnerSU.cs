using UnityEngine;

public class CircleSpawnerSU : MonoBehaviour
{
    private GameObject groupSU;
    private Vector3[] circlePoints;
    private int pointsIndex;
    private float LifeTime = 2f;

    private void Start()
    {
        pointsIndex = Random.Range(0, circlePoints.Length);
        Instantiate(groupSU, circlePoints[pointsIndex], transform.rotation);
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
