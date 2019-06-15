using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBase : MonoBehaviour
{
    [SerializeField] protected AudioClip push_sound;
    private AudioSource se_player;


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
        se_player.PlayOneShot(push_sound);
    }
}
