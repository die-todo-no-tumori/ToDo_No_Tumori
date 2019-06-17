﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBase : MonoBehaviour
{

    [SerializeField] protected string push_sound_name;
    private AudioClip push_sound;
    private AudioSource se_player;
    [SerializeField]
    protected GameObject target_object;
    [SerializeField]
    protected GameObject positive_target_object;
    [SerializeField]
    protected GameObject negative_target_object;


    protected virtual void Start()
    {
        if(push_sound_name != null && push_sound_name != "")
            push_sound = (AudioClip)Resources.Load("Audio/" + push_sound_name);
        se_player = Camera.main.gameObject.GetComponent<AudioSource>();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            PlaySE();
        });
    }

    void Update()
    {
        
    }

    private void PlaySE()
    {
        if(se_player != null && push_sound != null)
            se_player.PlayOneShot(push_sound);
    }
}
