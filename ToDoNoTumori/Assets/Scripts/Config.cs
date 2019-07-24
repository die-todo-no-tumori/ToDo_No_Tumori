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
    [SerializeField,Header("通知の何日前かのスライダー")]
    private Slider notification_days_slider;
    [SerializeField,Header("通知の何時のスライダー")]
    private Slider notification_hour_slider;
    [SerializeField]
    private Text volume_text;

    [HideInInspector]
    public static int notification_shift_days;
    [HideInInspector]
    public static int notification_hour;
    [SerializeField]
    private Text notification_shift_days_text;
    [SerializeField]
    private Text notification_hour_text;

    void Start()
    {
        se_slider.onValueChanged.AddListener(value =>
        {
            se_player.volume = value;
            if(volume_text != null)
                volume_text.text = "" + (int)(value * 10);
        });

        if(PlayerPrefs.HasKey("Nofitication_Shift_Days")){
            notification_shift_days = PlayerPrefs.GetInt("Nofitication_Shift_Days");
        }else
        {
            notification_shift_days = (int)notification_days_slider.value;
            PlayerPrefs.SetInt("Notification_Shift_Days",notification_shift_days);
        }
        

        if(PlayerPrefs.HasKey("Notification_Hour")){
            notification_shift_days = PlayerPrefs.GetInt("Nofitication_Hour");
        }else
        {
            notification_hour = (int)notification_hour_slider.value;
            PlayerPrefs.SetInt("Notification_Hour",notification_hour);
        }
        

        notification_days_slider.onValueChanged.AddListener(value => {
            notification_shift_days = (int)value;
            notification_shift_days_text.text = (notification_shift_days == 0) ? "当日" : (int)value + "日前";
        });

        notification_hour_slider.onValueChanged.AddListener(value => {
            notification_hour = (int)value;
            notification_hour_text.text = notification_hour + "時";
        });

    }



    void Update()
    {
        
    }
}
