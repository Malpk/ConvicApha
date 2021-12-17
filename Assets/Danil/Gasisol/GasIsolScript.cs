using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasIsolScript : MonoBehaviour
{

    public GameObject GasCloud;
    private bool ActiveRotate = false;
    private float zrot;
    public float LiveTime = 7;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 3)
        {
            
            GasCloud.SetActive(true);
            ActiveRotate = true;
           
        }

        if (LiveTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (ActiveRotate)
        {
            zrot = zrot + 10 * Time.fixedDeltaTime;
            GasCloud.transform.rotation = Quaternion.Euler(new Vector3(0, 0, zrot));
        }
        LiveTime = LiveTime - Time.fixedDeltaTime;
    }
}
