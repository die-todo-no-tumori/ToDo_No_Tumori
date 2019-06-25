using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBase : MonoBehaviour
{

    protected AudioClip push_sound;
    private AudioSource se_player;
    [SerializeField]
    protected GameObject target_object;


    protected virtual void Start()
    {
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
