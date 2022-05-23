using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private float spanwGap;
    [SerializeField] private float spawnTimer;
    [SerializeField] private KIFactory kiFactory;
    [SerializeField] private CircleCollider2D spawnZone;
    [SerializeField] private AnimationCurve difficultCurve;
    [SerializeField] private Tilemap tilemap;

    private Vector3 spawn;
    private Vector3Int spawnCell;
    private bool isTimerActive = false;
    private bool spawned = false;

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
            var device = kiFactory.GetRandomKI();
            spawn = (Random.insideUnitCircle * spawnZone.radius);
            spawn.x = spawn.x + spawnZone.transform.position.x; 
            spawn.y = spawn.y + spawnZone.transform.position.y;
            spawnCell = tilemap.WorldToCell(spawn);
            if(!(spawnCell.x < 0 || spawnCell.y < 0 || spawnCell.x > 19 || spawnCell.x > 19))
                device.transform.position = spawnCell;
            else
            {
                CorrectCoordinates();
                device.transform.position = spawnCell;
            }
                
            yield return new WaitForSeconds(spanwGap);
        }
    }

    private void CorrectCoordinates()
    {
        if (spawnCell.x < 0)
            spawnCell.x += 10;

        if (spawnCell.x > 19)
            spawnCell.x -= 10;  

        if (spawnCell.y < 0)      
            spawnCell.y += 10;  
        
        if (spawnCell.y > 19)
            spawnCell.y -= 10;
    }
    public void Activate()
    {
        StartTimer();
        StartCoroutine(DebugSpawn());
    }

    public void Deactivate()
    {
        StopAllCoroutines();
    }

    private void StartTimer()
    {
        isTimerActive = true;
    }

    
}
