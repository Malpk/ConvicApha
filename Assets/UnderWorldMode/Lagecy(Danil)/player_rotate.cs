using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_rotate : MonoBehaviour
{
    private float angle = 90;
   
    public GameObject bullet;
    public Transform shotPoint;
    public Transform shotPoint2;
    public float timeReload = 0;
    public float Reloadtime = 0.5f;
    private bool spc = true;
    private bool shotActive = false;


    Vector2 targetPos;
    public VariableJoystick variableJoystickR;

    void Update()
    {
        
        targetPos = new Vector2(variableJoystickR.Horizontal, variableJoystickR.Vertical);
        if (targetPos.x != 0 & targetPos.y != 0)
        {
           
            angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        }
    }

    private void FixedUpdate()
    {
        if (shotActive)
        {
            if (timeReload <= 0)
            {
                if (variableJoystickR.Vertical != 0 || variableJoystickR.Horizontal != 0)
                {
                    if (spc == true)
                    {
                        Instantiate(bullet, shotPoint.position, transform.rotation);
                        spc = false;
                    }
                    else
                    {
                        Instantiate(bullet, shotPoint2.position, transform.rotation);
                        spc = true;
                    }
                    timeReload = Reloadtime;
                }
            }
            else
            {
                timeReload -= Time.fixedDeltaTime;
            }
        }

    }
}
