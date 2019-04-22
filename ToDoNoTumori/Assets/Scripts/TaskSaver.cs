using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class TaskSaver : MonoBehaviour
{
    private TaskRoot taskRoot;
    void Start()
    {
        taskRoot = new TaskRoot();
    }

    void Update()
    {
        
    }

    public void SaveTask()
    {
        //存在するタスクをすべて取得
        foreach(TaskObject taskObject in transform.GetComponentsInChildren<TaskObject>())
        {
            taskRoot.tasks.Add(taskObject.task);
        }
        string data = JsonUtility.ToJson(taskRoot);
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/SaveData");
        if (directoryInfo.Exists == false)
            directoryInfo.Create();
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/SaveData/data.json");
        if (fileInfo.Exists == false)
            fileInfo.Create();
        using(StreamWriter streamWriter = new StreamWriter(Application.dataPath + "/SaveData/data.json"))
        {
            streamWriter.Write(data);
        }
    }
}
