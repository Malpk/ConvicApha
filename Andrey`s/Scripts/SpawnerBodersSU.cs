using UnityEngine;
using UnityEngine.UIElements;

public class SpawnerBodersSU : MonoBehaviour
{
    [SerializeField] private GameObject[] Borders;
    [SerializeField] private GameObject DownLeftPoint, UpRightPoint;
    private int bordersIndex, borderPositionX, borderPositionY;
    private float SpawnTime = 3f, minXPositionF, minYPositionF, maxXPositionF, maxYPositionF;
    private Vector2 borderPosition;

    private void Start()
    {  
        DownLeftPoint = GameObject.FindWithTag("DRPoint");
        UpRightPoint = GameObject.FindWithTag("ULPoint");       
    }
    void Update()
    {
        SpawnTime -= Time.deltaTime;

        if (SpawnTime <= 0)
        {
            int maxXPositionInt, minXPositionInt, maxYPositionInt, minYPositionInt;

            minXPositionF = DownLeftPoint.transform.position.x;
            minXPositionInt = (int) Mathf.Round(minXPositionF);       

            maxXPositionF = UpRightPoint.transform.position.x;
            maxXPositionInt = (int)Mathf.Round(maxXPositionF);

            minYPositionF = DownLeftPoint.transform.position.y;
            minYPositionInt = (int)Mathf.Round(minYPositionF);

            maxYPositionF = UpRightPoint.transform.position.y;
            maxYPositionInt = (int)Mathf.Round(maxYPositionF);

            borderPositionX = Random.Range(minXPositionInt, maxXPositionInt);
            borderPositionY = Random.Range(minYPositionInt, maxYPositionInt);

            borderPosition = new Vector2(borderPositionX, borderPositionY);
            bordersIndex = Random.Range(0,3);

            Instantiate(Borders[bordersIndex], borderPosition, transform.rotation);

            SpawnTime = 3f;
        }
    }
}
