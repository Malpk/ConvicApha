using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedDust : MonoBehaviour
{
    [SerializeField] private Transform dustInInventory;
    // Start is called before the first frame update
    void Start()
    {
        dustInInventory = GameObject.FindWithTag("Pos").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dirToInventory = (dustInInventory.position - transform.position).normalized;
            transform.position = (Vector2)transform.position + dirToInventory * Time.deltaTime * 50;

        float distance = Vector2.Distance(transform.position, dustInInventory.position);
        if (distance < 1)
        {
            Destroy(gameObject);
        }
    }
}
