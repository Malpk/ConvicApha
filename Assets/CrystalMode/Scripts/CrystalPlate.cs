using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPlate : MonoBehaviour
{
    [SerializeField] private float reloadTime;
    private CrystalModeInventory crystalModeInventory;
    private float timeSinceActivate;
    [SerializeField] private GameObject crystalDustAnim;
   
    void Start()
    {
        crystalModeInventory = Camera.main.GetComponent<CrystalModeInventory>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && timeSinceActivate > reloadTime)
        {
            timeSinceActivate = 0;
            Debug.Log("Give dust");
            ActivatePlate(1);
            var dust = Instantiate(crystalDustAnim);
            dust.transform.position = transform.position;
        }
    }
    
    private void ActivatePlate(int count)
    {
        crystalModeInventory.AddDustToInventory(count);
    }
}
