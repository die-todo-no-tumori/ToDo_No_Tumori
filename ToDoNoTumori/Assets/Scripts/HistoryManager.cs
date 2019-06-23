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

    void Start()
    {
        scrollRect.horizontal = false;
        Vector2 rectPos = scrollRect.content.GetComponent<RectTransform>().localPosition;
        rectPos.y = -50;
        scrollRect.content.GetComponent<RectTransform>().localPosition = rectPos;
        //for (int i = 0; i < 51; i++)
        //{
        //    GameObject button = Instantiate(history_object, scrollRect.content.transform);
        //    Vector2 pos = scrollRect.content.transform.position;
        //    pos.x = 720;
        //    pos.y -= (history_object.GetComponent<RectTransform>().rect.height) * i;
        //    button.GetComponent<RectTransform>().position = pos;
        //}
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
    }

    //入力履歴に追加
    public void AddToInputHistory(TaskData taskData)
    {
        int objectCount = spawn_origin_input_history.transform.childCount;
        GameObject obje = Instantiate(history_object, spawn_origin_input_history.GetComponent<RectTransform>().position, Quaternion.identity);
        RectTransform objeRect = obje.GetComponent<RectTransform>();
        Vector2 spawnPos = spawn_origin_input_history.GetComponent<RectTransform>().position;
        spawnPos.y -= objeRect.rect.height * objectCount;
        objeRect.position = spawnPos;
        objeRect.SetParent(spawn_origin_input_history.GetComponent<RectTransform>());
        objeRect.transform.localScale = Vector3.one;
        obje.GetComponent<HistoryObject>().task_data = taskData;
        obje.GetComponent<HistoryObject>().application_user = application_user;
        obje.transform.GetChild(0).GetComponent<RawImage>().texture = taskData.texture2D;
    }

    //破壊履歴に追加
    public void AddToDestroyHistory(TaskData taskData)
    {
        int objectCount = spawn_origin_destroy_history.transform.childCount;
        GameObject obje = Instantiate(history_object, spawn_origin_destroy_history.GetComponent<RectTransform>().position, Quaternion.identity);
        RectTransform objeRect = obje.GetComponent<RectTransform>();
        Vector2 spawnPos = spawn_origin_destroy_history.GetComponent<RectTransform>().position;
        spawnPos.y -= objeRect.rect.height * objectCount;
        objeRect.position = spawnPos;
        objeRect.SetParent(spawn_origin_destroy_history.GetComponent<RectTransform>());
        objeRect.transform.localScale = Vector3.one;
        obje.GetComponent<HistoryObject>().task_data = taskData;
        obje.GetComponent<HistoryObject>().application_user = application_user;
    }

    //総合履歴に追加
    public void AddToTotalHisttory(TaskData taskData)
    {
        int objectCount = spawn_origin_total_history.transform.childCount;
        GameObject obje = Instantiate(history_object, spawn_origin_total_history.GetComponent<RectTransform>().position, Quaternion.identity);
        RectTransform objeRect = obje.GetComponent<RectTransform>();
        Vector2 spawnPos = spawn_origin_total_history.GetComponent<RectTransform>().position;
        spawnPos.y -= objeRect.rect.height * objectCount;
        objeRect.position = spawnPos;
        objeRect.SetParent(spawn_origin_total_history.GetComponent<RectTransform>());
        objeRect.transform.localScale = Vector3.one;
        obje.GetComponent<HistoryObject>().task_data = taskData;
        obje.GetComponent<HistoryObject>().application_user = application_user;
    }
}
