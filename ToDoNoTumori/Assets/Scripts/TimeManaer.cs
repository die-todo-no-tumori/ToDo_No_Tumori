using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManaer : MonoBehaviour
{
    private string last_run_time;


    IEnumerator Start()
    {
        yield return null;
        if (PlayerPrefs.HasKey("last"))
        {
            last_run_time = PlayerPrefs.GetString("last");
            //日付を比較する
            DateTime last_time = DateTime.Parse(last_run_time);
            last_time = new DateTime(last_time.Year, last_time.Month, last_time.Day, 0, 0, 0);

            if(DateTime.Now.CompareTo(last_time) != 0)
            {
                GameObject[] tasks = GameObject.FindGameObjectsWithTag("Task");
                foreach(GameObject task in tasks)
                {
                    task.GetComponent<TaskObject>().ChangeColor();
                }
            }
        }
        last_run_time = DateTime.Now.ToString();
        PlayerPrefs.SetString("last", last_run_time);
    }

    
}
