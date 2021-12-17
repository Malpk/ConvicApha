using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamScript : MonoBehaviour
{

    public GameObject Player;
    public bool up;
    public bool right;
    public bool down;
    public bool left;
    private bool CanPush=false;
    private Rigidbody2D rb;
    private Player PlayerWasd;
    private float PushReload;
    private float PushReloadtime = 0.15f;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = Player.GetComponent<Rigidbody2D>();
        PlayerWasd = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (CanPush)
        {
            if (PushReload <= 0)
            {
               // if (up) { rb.MovePosition(rb.position + Vector2.up * PlayerWasd._speedMovement * Time.fixedDeltaTime); PlayerWasd.CanMove = false; }
               // else if (right) { rb.MovePosition(rb.position + Vector2.right * PlayerWasd._speedMovement * Time.fixedDeltaTime); PlayerWasd.CanMove = false; }
               // else if (down) { rb.MovePosition(rb.position + -Vector2.up * PlayerWasd._speedMovement * Time.fixedDeltaTime); PlayerWasd.CanMove = false;  }
              //  else if (left) { rb.MovePosition(rb.position + -Vector2.right * PlayerWasd._speedMovement * Time.fixedDeltaTime); PlayerWasd.CanMove = false; }
                
                Debug.Log(Player);
                Debug.Log(rb);
                Debug.Log(PlayerWasd.CanMove);
            }
        }
        else
        {
           
        }
    }

    private void FixedUpdate()
    {
        PushReload = PushReload - Time.fixedDeltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            CanPush = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PushReload = PushReloadtime;
            CanPush = false;
            PlayerWasd.CanMove = true;
        }
    }
}
