using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAnimation : MonoBehaviour
{
    public void OnFireDestroy()
    {
        Destroy(gameObject);
    }
}
