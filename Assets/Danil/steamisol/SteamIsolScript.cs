using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamIsolScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Steam1;
    public GameObject Steam2;
    public GameObject Steam3;
    public GameObject Steam4;
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
            Steam1.SetActive(true);
            Steam2.SetActive(true);
            Steam3.SetActive(true);
            Steam4.SetActive(true);
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
