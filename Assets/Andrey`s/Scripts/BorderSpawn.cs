using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderSpawn : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float distance = 10f;
    [SerializeField] private GameObject[] Borders;
    public static float  bordersSpawnLifeTime = 2f;
    private int xPos;
    private int yPos;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        StartCoroutine(PeriodicSpawn());
    }

    private IEnumerator PeriodicSpawn()
    {
        float lifeStart = bordersSpawnLifeTime;

        while(true)
        {
            yield return new WaitForSeconds(lifeStart);

            int bordersIndex = Random.Range(0, Borders.Length);

            float minXPosF = player.transform.position.x - distance;
            int minXPos = (int)Mathf.Round(minXPosF);

            float maxXPosF = player.transform.position.x + distance;
            int maxXPos = (int)Mathf.Round(maxXPosF);

            float minYPosF = player.transform.position.y - distance;
            int minYPos = (int)Mathf.Round(minYPosF);

            float maxYPosF = player.transform.position.y + distance;
            int maxYPos = (int)Mathf.Round(maxYPosF);

            xPos = Random.Range(minXPos, maxXPos);
            yPos = Random.Range(minYPos, maxYPos);

            Vector2 spawnPoint = new Vector2(xPos, yPos);

            Vector2 _corner1 = new Vector2(xPos - 2.5f, yPos + 2.5f);
            Vector2 _corner2 = new Vector2(xPos + 2.5f, yPos - 2.5f);

            LayerMask mask = LayerMask.GetMask("GroupSU");

            if (Physics2D.OverlapArea(_corner1, _corner2, mask) == null)
            {
                Instantiate(Borders[bordersIndex], spawnPoint, transform.rotation);
            }
            else 
            {
                xPos = Random.Range(minXPos, maxXPos);
                yPos = Random.Range(minYPos, maxYPos);

                spawnPoint = new Vector2(xPos, yPos);

                transform.position = spawnPoint;
            }
        }
        
    }
}