using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Task
{
    [HideInInspector] public string taskName;
    [HideInInspector] public int priority;
    [HideInInspector] public string limit;
}

public class TaskObject : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
