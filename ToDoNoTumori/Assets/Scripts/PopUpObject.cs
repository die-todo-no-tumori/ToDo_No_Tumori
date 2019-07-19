﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PopUpObject : MonoBehaviour
{
    [SerializeField]
    private Text detail_text;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //吹き出しを非表示にするメソッド
    public void Hide(){
        detail_text.text = "";
        gameObject.SetActive(false);
    }

    //吹き出しに内容を表示するメソッド
    public void Show(TaskData taskData)
    {
        string[] limitData = taskData.task_limit.Split('/');
        detail_text.text = "" + limitData[1] + "月" + limitData[2] + "日";
        // Debug.Log(taskData.task_limit);
        DateTime time = DateTime.Parse(taskData.task_limit);
        TimeSpan span = time - DateTime.Today;
        string message = "" +( span.Days == 0 ? "当日" : "あと" + span.Days + "日");
        detail_text.text += Environment.NewLine + message;
    }


}
