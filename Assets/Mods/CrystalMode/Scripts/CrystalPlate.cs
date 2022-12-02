using System;
using UnityEngine;

public class CrystalPlate : MonoBehaviour
{
    [SerializeField] private float reloadTime;
    private float timeSinceActivate;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Player>() && timeSinceActivate > reloadTime)
        {
            timeSinceActivate = 0;
            //give 1 dust
        }
    }

    private void Update()
    {
        timeSinceActivate += Time.deltaTime;
    }
}
