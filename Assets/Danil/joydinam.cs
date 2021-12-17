using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class joydinam : MonoBehaviour
{

    public GameObject sosok;
    public GameObject bk;
    public Sprite sosoknorm;
    public Sprite sosokstrel;
    public VariableJoystick variableJoystick;
    public Canvas can;
    private Vector2 targetPos;
    public float distant;
    void Update()
    {
        distant = Vector2.Distance(sosok.transform.position,bk.transform.position)/ can.scaleFactor;
        if(Vector2.Distance(sosok.transform.position, bk.transform.position) / can.scaleFactor > 50)
        {
            targetPos = new Vector2(variableJoystick.Horizontal, variableJoystick.Vertical);
            float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle -90 ));
            sosok.GetComponent<Image>().sprite = sosokstrel;
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            sosok.GetComponent<Image>().sprite = sosoknorm;
        }
       

    }
}
