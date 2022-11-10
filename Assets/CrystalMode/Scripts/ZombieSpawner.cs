using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawner : MonoBehaviour
{
    List<Transform> points = new List<Transform>();
    private void Start()
    {
        GameObject[] pointsObj = GameObject.FindGameObjectsWithTag("Point");
        foreach (var point in pointsObj)
        {
            points.Add(point.transform);
        }
    }

    public IEnumerator SpawnEnemy(float secondsToWait, GameObject pref)
    {
        GameObject enemy = Instantiate(pref);
        Transform choosedPoint = points[Random.Range(0, points.Count)];
        enemy.transform.position = choosedPoint.position;
        enemy.SetActive(false);
        yield return new WaitForSeconds(secondsToWait);
        enemy.SetActive(true);
    }

    public void SpawnEnemyVoid(float secondsToWait, GameObject enemy)
    {
        StartCoroutine(SpawnEnemy(secondsToWait, enemy));
    }
}
