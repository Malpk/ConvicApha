using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private KIFactory kIFactory;
    [SerializeField] private CircleCollider2D spawnZone;
    [SerializeField] private float spanwGap;
    [SerializeField] private AnimationCurve difficultCurve;
    [SerializeField] private float spawnTimer;

    private bool isTimerActive = false;

    private void Update()
    {
        if (!isTimerActive)
            return;
        spanwGap = difficultCurve.Evaluate(spawnTimer);
        spawnTimer += Time.deltaTime;
    }
    private IEnumerator DebugSpawn()
    {
        while (true)
        {
            var device = kIFactory.GetGun(GunsEnum.RocketLauncher);
            Vector2 spawn = (Random.insideUnitCircle * spawnZone.radius);  
            spawn.x += spawnZone.transform.position.x; 
            spawn.y += spawnZone.transform.position.y;

            device.transform.position = new Vector3(spawn.x, spawn.y);
            yield return new WaitForSeconds(spanwGap);
        }
    }

    public void Activate()
    {
        StartTimer();
        StartCoroutine(DebugSpawn());
    }

    private void StartTimer()
    {
        isTimerActive = true;
    }
}
