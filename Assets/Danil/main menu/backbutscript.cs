﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backbutscript : MonoBehaviour
{
    public GameObject button;
    public Sprite anbut1;
    public Sprite anbut2;
    public GameObject menu;
    public GameObject setmenu;
    public GameObject playmenu;
    public AudioClip clikc;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        button.GetComponent<SpriteRenderer>().sprite = anbut2;
        aud(clikc);
    }
    private void OnMouseUp()
    {
        button.GetComponent<SpriteRenderer>().sprite = anbut1;

        setmenu.SetActive(false);
        playmenu.SetActive(false);


        menu.SetActive(true);

    }

    public void aud(AudioClip clipp)
    {
        button.GetComponent<AudioSource>().PlayOneShot(clipp);
    }



}
