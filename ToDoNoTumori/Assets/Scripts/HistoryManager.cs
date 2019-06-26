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
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private ApplicationUser application_user;
    [SerializeField]
    private ScrollRect scrollRect;
    private GameObject catching_object;

    void Start()
    {
        scrollRect.horizontal = false;
        Vector2 rectPos = scrollRect.content.GetComponent<RectTransform>().localPosition;
        rectPos.y = -50;
        scrollRect.content.GetComponent<RectTransform>().localPosition = rectPos;
    }


    void Update()
    {
        if (scrollRect.content.GetComponent<RectTransform>().localPosition.y <= -50)
        {
            Vector2 pos = scrollRect.content.GetComponent<RectTransform>().localPosition;
            pos.y = -50;
            scrollRect.content.GetComponent<RectTransform>().localPosition = pos;
        }

        if (scrollRect.content.GetComponent<RectTransform>().localPosition.y >= 2300)
        {
            Vector2 pos = scrollRect.content.GetComponent<RectTransform>().localPosition;
            pos.y = 2300;
            scrollRect.content.GetComponent<RectTransform>().localPosition = pos;
        }

        //指が1本触れているとき
        if(Input.touchCount == 1)
        {
            //
            if(catching_object == null)
            {

            }
        }
    }


    //入力履歴に追加
    public void AddToInputHistory(TaskData taskData)
    {
        int objectCount = spawn_origin_input_history.transform.parent.childCount - 1;
        //上から何番目なのか
        int horizontal = objectCount % 3;
        //左から何番目なのか
        int vertical = objectCount / 3;
        int dist = 520;

        GameObject obje = Instantiate(history_object, spawn_origin_input_history.GetComponent<RectTransform>().position, Quaternion.identity);
        RectTransform objeRect = obje.GetComponent<RectTransform>();
        Vector2 spawnPos = spawn_origin_input_history.GetComponent<RectTransform>().position;
        //spawnPos.y -= objeRect.rect.height * objectCount;
        spawnPos.x += horizontal * dist;
        spawnPos.y -= vertical * dist;
        objeRect.position = spawnPos;
        objeRect.SetParent(spawn_origin_input_history.GetComponent<RectTransform>().parent);
        objeRect.transform.localScale = Vector3.one;
        obje.GetComponent<HistoryObject>().task_data = taskData;
        obje.GetComponent<HistoryObject>().application_user = application_user;
        obje.transform.GetChild(0).GetComponent<RawImage>().texture = taskData.texture2D;
    }

    //入力履歴から削除
    public void RemoveFromInputHistory(HistoryObject historyObject)
    {
        Transform content_rect = spawn_origin_input_history.transform.parent;
        TaskData targetData = null;
        foreach(HistoryObject target in content_rect.GetComponentsInChildren<HistoryObject>())
        {
            if(target == historyObject)
            {
                targetData = target.task_data;
            }
        }



        if(targetData != null)
            AddToDestroyHistory(targetData);
    }

    //破壊履歴に追加
    public void AddToDestroyHistory(TaskData taskData)
    {
        int objectCount = spawn_origin_destroy_history.transform.parent.childCount - 1;
        //上から何番目なのか
        int horizontal = objectCount % 3;
        //左から何番目なのか
        int vertical = objectCount / 3;
        int dist = 520;

        GameObject obje = Instantiate(history_object, spawn_origin_destroy_history.GetComponent<RectTransform>().position, Quaternion.identity);
        RectTransform objeRect = obje.GetComponent<RectTransform>();
        Vector2 spawnPos = spawn_origin_destroy_history.GetComponent<RectTransform>().position;
        //spawnPos.y -= objeRect.rect.height * objectCount;
        spawnPos.x += horizontal * dist;
        spawnPos.y -= vertical * dist;
        objeRect.position = spawnPos;
        objeRect.SetParent(spawn_origin_destroy_history.GetComponent<RectTransform>().parent);
        objeRect.transform.localScale = Vector3.one;
        obje.GetComponent<HistoryObject>().task_data = taskData;
        obje.GetComponent<HistoryObject>().application_user = application_user;
        obje.transform.GetChild(0).GetComponent<RawImage>().texture = taskData.texture2D;
    }

    //破壊履歴から削除
    public void RemoveFromDestroyHistory(HistoryObject historyObject)
    {
        Transform content_rect = spawn_origin_destroy_history.transform.parent;
        TaskData targetData = null;
        foreach (HistoryObject target in content_rect.GetComponentsInChildren<HistoryObject>())
        {
            if (target == historyObject)
            {
                targetData = target.task_data;
            }
        }
        //if (targetData != null)
        //    AddToDestroyHistory(targetData);
    }

    //総合履歴に追加
    public void AddToTotalHisttory(TaskData taskData)
    {
        int objectCount = spawn_origin_total_history.transform.parent.childCount - 1;
        //上から何番目なのか
        int horizontal = objectCount % 3;
        //左から何番目なのか
        int vertical = objectCount / 3;
        int dist = 520;

        GameObject obje = Instantiate(history_object, spawn_origin_total_history.GetComponent<RectTransform>().position, Quaternion.identity);
        RectTransform objeRect = obje.GetComponent<RectTransform>();
        Vector2 spawnPos = spawn_origin_total_history.GetComponent<RectTransform>().position;
        //spawnPos.y -= objeRect.rect.height * objectCount;
        spawnPos.x += horizontal * dist;
        spawnPos.y -= vertical * dist;
        objeRect.position = spawnPos;
        objeRect.SetParent(spawn_origin_total_history.GetComponent<RectTransform>().parent);
        objeRect.transform.localScale = Vector3.one;
        obje.GetComponent<HistoryObject>().task_data = taskData;
        obje.GetComponent<HistoryObject>().application_user = application_user;
        obje.transform.GetChild(0).GetComponent<RawImage>().texture = taskData.texture2D;
    }

    //総合履歴から削除
    public void RemoveFromTotalHistory(HistoryObject historyObject)
    {
        Transform content_rect = spawn_origin_input_history.transform.parent;
        TaskData targetData = null;
        foreach (HistoryObject target in content_rect.GetComponentsInChildren<HistoryObject>())
        {
            if (target == historyObject)
            {
                targetData = target.task_data;
            }
        }
        //if (targetData != null)
        //    AddToDestroyHistory(targetData);
    }
}
