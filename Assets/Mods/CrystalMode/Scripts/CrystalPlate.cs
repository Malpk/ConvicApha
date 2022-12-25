using System;
using MainMode.Items;
using PlayerComponent;
using UnityEngine;

public class CrystalPlate : MonoBehaviour
{
    [SerializeField] private float reloadTime;
    [SerializeField] private CrystalDust crystalDust;
    private float timeSinceActivate;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Player>() && timeSinceActivate > reloadTime)
        {
            Debug.Log("Add");
            timeSinceActivate = 0;
            col.gameObject.GetComponent<PlayerInventory>().AddConsumablesItem(Instantiate(crystalDust));
            gameObject.GetComponent<Animator>().Play("CrystalPlateReload");
        }
    }

    private void Update()
    {
        timeSinceActivate += Time.deltaTime;
    }
}
