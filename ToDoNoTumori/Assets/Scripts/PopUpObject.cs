using System.Collections;
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

    //吹き出しに内容を表示するメソッド
    public void Show(TaskData taskData)
    {
        detail_text.text = "" + taskData.task_limit;
        DateTime time = DateTime.Parse(taskData.task_limit);
        TimeSpan span = time - DateTime.Today;
        string message = "" +( span.Days == 0 ? "当日" : "あと" + span.Days + "日");
        detail_text.text += Environment.NewLine + message;
    }


}
