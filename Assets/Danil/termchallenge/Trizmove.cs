﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trizmove : MonoBehaviour
{
    [SerializeField] private float _timeDestroy;
    [SerializeField] private float _speeMovementd;

    void Start()
    {
        if(_timeDestroy!=0)
            Destroy(gameObject, _timeDestroy);
    }

    void Update()
    {
        transform.Translate(Vector2.down * _speeMovementd * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Player>().Dead();
        }
    }
}
