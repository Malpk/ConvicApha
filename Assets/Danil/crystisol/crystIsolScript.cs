using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystIsolScript : MonoBehaviour
{
    public GameObject Wall1;
    public GameObject Wall2;
    public GameObject Wall3;
    public GameObject Wall4;
    public float LiveTime = 7;

    void Update()
    {
        if(Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 3)
        {
            Wall1.SetActive(true);
            Wall2.SetActive(true);
            Wall3.SetActive(true);
            Wall4.SetActive(true);
        }

        if (LiveTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {    
        LiveTime = LiveTime - Time.fixedDeltaTime;
    }
}
