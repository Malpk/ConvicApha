using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitsOfspLookAt : MonoBehaviour
{
    private GameObject player;
    private void Start()
    {
       player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        transform.position = player.transform.position;
    }
}
