using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet1 : MonoBehaviour
{
    public float speed;
    public float Lifetime;
    public float distance;
    public int damage;
    public LayerMask isSol;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.up, distance, isSol);
        if (hitinfo.collider != null)
        {
            if (hitinfo.collider.CompareTag("Enemy"))
            {
                GameObject ev = hitinfo.collider.gameObject;
                Destroy(ev);
            }
            Destroy(gameObject);
        }
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime);
    }
}
