using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Task
{
    [HideInInspector] public string taskName;
    [HideInInspector] public int priority;
    [HideInInspector] public string limit;

    public Task(string name , int pri, string lim)
    {
        this.taskName = name;
        this.priority = pri;
        this.limit = lim;
    }
}

public class TaskObject : MonoBehaviour
{
    [HideInInspector] public Task task;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
