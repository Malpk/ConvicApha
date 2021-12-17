using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateTypeGen : MonoBehaviour
{
    public float TimeToGen = 0;

    public bool NonSpritePlate = true;

    public GameObject[] PlateType = new GameObject[4];




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeToGen >= 1)
        {
            

                
                Instantiate(PlateType[Random.Range(0, PlateType.Length )],transform.position,Quaternion.identity);
                Destroy(gameObject);

              
        }
        
    }
    private void FixedUpdate()
    {
        if (TimeToGen >= 0)
        {
            TimeToGen = TimeToGen + Time.fixedDeltaTime;
        }
        
    }
}
