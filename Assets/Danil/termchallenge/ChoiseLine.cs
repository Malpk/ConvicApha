using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiseLine : MonoBehaviour
{

    public float timetoActive = 3;
    private GameObject cho1;
    private GameObject cho2;
    private GameObject cho3;
    private GameObject cho4;
    private GameObject cho5;
    private GameObject cho6;
    public GameObject[] lines = new GameObject[4];
    
    public GameObject triz;
    // Start is called before the first frame update
    void Start()
    {
        RandomChaise();
    }
    private void FixedUpdate()
    {
        if (timetoActive <= 0)
        {
            
            if (cho1.GetComponent<SpriteRenderer>().color.a < 0.4f)
            {
                cho1.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, cho1.GetComponent<SpriteRenderer>().color.a + 0.1f * Time.fixedDeltaTime);
            }
            else
            {
                if (cho1 == lines[0] || cho1 == lines[2] || cho1 == lines[4] || cho1 == lines[6])
                {
                    Instantiate(triz, new Vector2(cho1.transform.position.x-30, cho1.transform.position.y), Quaternion.Euler(0, 0,-90));
                    cho1.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                }
                if (cho1 == lines[1] || cho1 == lines[3] || cho1 == lines[5] || cho1 == lines[7])
                {
                    Instantiate(triz, new Vector2(cho1.transform.position.x + 30, cho1.transform.position.y),Quaternion.Euler(0,0,90));
                    cho1.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);

                }
                
            }

            if (cho2.GetComponent<SpriteRenderer>().color.a < 0.4f)
            {
                cho2.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, cho2.GetComponent<SpriteRenderer>().color.a + 0.1f * Time.fixedDeltaTime);
            }
            else
            {
                if (cho2 == lines[0] || cho2 == lines[2])
                {
                    Instantiate(triz, new Vector2(cho2.transform.position.x - 30, cho2.transform.position.y), Quaternion.Euler(0, 0, -90));
                    cho2.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                }
                if (cho2 == lines[1] || cho2 == lines[3])
                {
                    Instantiate(triz, new Vector2(cho2.transform.position.x + 30, cho2.transform.position.y), Quaternion.Euler(0, 0, 90));
                    cho2.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);

                }
                timetoActive = 3;
                RandomChaise();
            }

        }

        timetoActive = timetoActive - Time.fixedDeltaTime;
    }

    public void RandomChaise()
    {
        cho1 = lines[Random.Range(0, 4)];
        for(bool serch = true; serch ==true;)
        {
            GameObject i = lines[Random.Range(0, 8)];
            if (i != cho1)
            {
                cho2 = i;
                serch = false;
            }
        }
        for (bool serch = true; serch == true;)
        {
            GameObject i = lines[Random.Range(0, 8)];
            if (i != cho1 || i != cho2)
            {
                cho3 = i;
                serch = false;
            }
        }
        for (bool serch = true; serch == true;)
        {
            GameObject i = lines[Random.Range(0, 8)];
            if (i != cho1 || i != cho2 || i != cho3)
            {
                cho4 = i;
                serch = false;
            }
        }
        for (bool serch = true; serch == true;)
        {
            GameObject i = lines[Random.Range(0, 8)];
            if (i != cho1 || i != cho2 || i != cho3 || i != cho4)
            {
                cho5 = i;
                serch = false;
            }
        }
        for (bool serch = true; serch == true;)
        {
            GameObject i = lines[Random.Range(0, 8)];
            if (i != cho1 || i != cho2 || i != cho3 || i != cho4 || i != cho5)
            {
                cho6 = i;
                serch = false;
            }
        }
    }
}
