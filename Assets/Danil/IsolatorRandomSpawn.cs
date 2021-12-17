using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolatorRandomSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Isolators = new GameObject[2];
    
    public LayerMask isPlate;
    private Vector2[] FreePlatePlace = new Vector2[0];

    private float Spawnx;
    private float Spawny;
    private int PlateCounter = 0;
    private Vector2 randomChoice;


    public float ReloadTime = 1f;
    private float ReloadDin;
    // Start is called before the first frame update
    void Start()
    {
        ReloadDin = ReloadTime;

    }

    private void FixedUpdate()
    {
        if (ReloadDin > 0)
        {
            ReloadDin = ReloadDin - Time.fixedDeltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (ReloadDin <= 0)
        {
            SpawnPlate();
            ReloadDin = ReloadTime;
        }

    }
    public void SpawnPlate()
    {
        FreePlatePlace = new Vector2[0];
        Array.Resize(ref FreePlatePlace, 0);
        PlateCounter = 0;
        for (float sx = -11.6f; sx <= 10f; sx = sx + 0.8f)
        {


            Spawnx = 0.8f + (transform.position.x + (float)Math.Round(sx, 2)) - ((transform.position.x + (float)Math.Round(sx, 2)) % 1.6f);

            for (float sy = -11.6f; sy <= 10f; sy = sy + 0.8f)
            {
                Spawny = 0.8f + (transform.position.y + (float)Math.Round(sy, 2)) - ((transform.position.y + (float)Math.Round(sy, 2)) % 1.6f);

                RaycastHit2D hitinfo = Physics2D.Raycast(new Vector2(Spawnx, Spawny), transform.forward, 1000, isPlate);

                if (hitinfo.collider == null)
                {
                    if ((Spawnx <= transform.position.x - 5.6f || Spawnx >= transform.position.x+ 5.6f) || (Spawny <= transform.position.y - 5.6f || Spawny >= transform.position.y+ 5.6f))
                    {
                       
                            Array.Resize(ref FreePlatePlace, PlateCounter + 1);
                            FreePlatePlace[PlateCounter] = new Vector2(Spawnx, Spawny);
                            PlateCounter++;
                        
                    }
                }
            }
        }



        if (FreePlatePlace.Length != 0)
        {
            randomChoice = FreePlatePlace[UnityEngine.Random.Range(0, FreePlatePlace.Length - 1)];
            Instantiate(Isolators[UnityEngine.Random.Range(0,Isolators.Length)], new Vector3(randomChoice.x, randomChoice.y, 5), Quaternion.Euler(0, 0, 0));
        }
        else
        {
            Debug.Log("клеточек нету");
        }


    }
}
