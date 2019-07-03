using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Config : MonoBehaviour
{
    [SerializeField]
    private AudioSource se_player;
    [SerializeField]
    private Slider se_slider;
    [SerializeField]
    private Text volume_text;

    void Start()
    {
        se_slider.onValueChanged.AddListener(value =>
        {
            se_player.volume = value;//(int)(value / 0.1f) / 10;
            if(volume_text != null)
                volume_text.text = "" + (int)(se_player.volume * 10);
        });
    }



    void Update()
    {
        
    }
}
