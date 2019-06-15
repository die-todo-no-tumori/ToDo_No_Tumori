﻿using UnityEngine;
using System.IO;

public class SaveAndLoader : MonoBehaviour
{
    private TaskRoot task_root;


    void Start()
    {
        //タスクデータを読み込み、クラスを作成
        
        //生成
    }

    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        //タスクデータを収集

        //タスクデータを書き込み

    }


    //タスクデータの読み込み
    private string ReadTaskData()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/Data");
        if(directoryInfo.Exists == false)
            return null;

        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Data/Data.json");
        if (fileInfo.Exists == false)
            return null;

        using(StreamReader streamReader = new StreamReader(fileInfo.FullName))
        {
            string data = streamReader.ReadToEnd();
            return data;
        }
    }
    
    
    //読み込んだタスクデータをタスククラスに変換
    private TaskRoot ConvertToTaskData(string data)
    {
        task_root = new TaskRoot();
        task_root = JsonUtility.FromJson<TaskRoot>(data);
        return task_root;
    }


    //タスクデータを持つタスクオブジェクトを生成する
    private void CreateTaskObjects(GameObject taskObjePrefab,TaskRoot taskRoot , GameObject spawnPos)
    {
        foreach(TaskData taskData in taskRoot.task_datas)
        {
            GameObject task_object = Instantiate(taskObjePrefab, spawnPos.transform.position, Quaternion.identity);
            task_object.GetComponent<TaskObject>().task_data = taskData;
        }
    }


    //タスクオブジェクトを探索してタスクデータを収集
    private TaskRoot CollectTaskDatas()
    {
        TaskRoot task_root = new TaskRoot();
        GameObject[] task_objects = GameObject.FindGameObjectsWithTag("TaskObject");
        foreach(GameObject task_object in task_objects)
        {
            task_root.task_datas.Add(task_object.GetComponent<TaskObject>().task_data);
        }
        return task_root;
    }

    //タスクデータを書き込み
    private void WriteTaskData(TaskRoot taskRoot)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/Data");
        if (directoryInfo.Exists == false)
            directoryInfo.Create();

        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Data/Data.json");
        if (fileInfo.Exists == false)
            fileInfo.Create();

        string write_data = JsonUtility.ToJson(taskRoot);
        using(StreamWriter streamWriter = new StreamWriter(fileInfo.FullName))
        {
            streamWriter.Write(write_data);
        }
    }
}
