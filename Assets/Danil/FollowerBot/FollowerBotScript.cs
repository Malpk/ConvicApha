using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerBotScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject Player;
    private float angle;
    private float x;
    private float y;
    private Vector2 SpeedVector =  new Vector2(0,0);

    public float InerPower =0.75f;
    public float MoveSpeed = 0.4f;
    public float MaxVectorSpeed = 5;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    { 
        if (transform.eulerAngles.z <= 180)
        {
            y = 1 - transform.eulerAngles.z / 90;
            
        }
        else
        {
            y = (1 - (transform.eulerAngles.z-180) / 90)* -1;
            
        }

        if (y >= 0)
        {
            x = 1 - y;
        }
        else
        {
            x = 1 + y;
        }
        if(transform.eulerAngles.z <= 180)
        {
            x = x * -1;
        }

        angle = Mathf.Atan2(Player.transform.position.y - transform.position.y, 
                            Player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));

        SpeedVector = new Vector2(SpeedVector.x + x* InerPower, SpeedVector.y + y * InerPower);
        SpeedVector = new Vector2(Mathf.Clamp(SpeedVector.x, -MaxVectorSpeed, MaxVectorSpeed), 
                                  Mathf.Clamp(SpeedVector.y, -MaxVectorSpeed, MaxVectorSpeed));

        rb.MovePosition(rb.position + SpeedVector* MoveSpeed * Time.fixedDeltaTime);
    }
}
