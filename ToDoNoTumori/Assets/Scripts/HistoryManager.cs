using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject history_object;
    [SerializeField]
    private GameObject spawn_origin_input_history;
    [SerializeField]
    private GameObject spawn_origin_destroy_history;
    [SerializeField]
    private GameObject spawn_origin_total_history;

    void Start()
    {
        
    }



    void Update()
    {
        
    }

    public void AddToInputHistory(TaskData taskData)
    {
        int objectCount = spawn_origin_input_history.transform.GetChildCount();
        GameObject obje = Instantiate(history_object, spawn_origin_input_history.GetComponent<RectTransform>().position, Quaternion.identity);
        RectTransform objeRect = obje.GetComponent<RectTransform>();
        Vector2 spawnPos = spawn_origin_input_history.GetComponent<RectTransform>().position;
        spawnPos.y -= objeRect.rect.height * objectCount;
        objeRect.position = spawnPos;
        objeRect.SetParent(spawn_origin_input_history.GetComponent<RectTransform>());
        
    }

    public void AddToDestroyHistory(TaskData taskData)
    {
        int objectCount = spawn_origin_destroy_history.transform.GetChildCount();
    }

    public void AddToTotalHisttory(TaskData taskData)
    {
        int objectCount = spawn_origin_total_history.transform.GetChildCount();
    }
}
