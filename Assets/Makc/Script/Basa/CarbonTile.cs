using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbonTile : MonoBehaviour
{
    [Header("Game Setting")]
    [SerializeField] private float _workingHours;

    private void Start()
    {
        Destroy(gameObject, _workingHours);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
