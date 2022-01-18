using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;

public class PlateActives : MonoBehaviour
{
    public bool PlateN7 = false;
    public bool PlateC14 = false;
    public bool PlateTI81 = false;
    public bool PlateU92 = false;
    public bool PlateTerm = false;

    private GameObject Player;
    private Player Playerwasd;

    public float LiveTime = 20;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Playerwasd = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(LiveTime <= 0) { Destroy(gameObject); }
    }

    private void FixedUpdate()
    {
        LiveTime = LiveTime - Time.fixedDeltaTime;
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
          //  if(PlateN7) Playerwasd.N7();
          //  if (PlateC14) Playerwasd.C14();
          //  if (PlateTI81) Playerwasd.TI81();
          //  if (PlateU92) Playerwasd.U92();
         // if (PlateTerm) Playerwasd.Term();
        }
    }
}
