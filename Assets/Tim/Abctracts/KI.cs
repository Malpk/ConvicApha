using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KI : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<KI>() is KI)
        {
            Destroy(collision.gameObject);
        }
    }
}
