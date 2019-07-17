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
    private RectTransform input_history_panel_rect;
    [SerializeField]
    private RectTransform destroy_history_panel_rect;
    [SerializeField]
    private RectTransform total_history_panel_rect;
    private GameObject catching_object;

    private int dist_ver;
    private int[] dist_hor;
    void Start()
    {
        dist_ver = (Screen.height - 150) / 6;
        dist_hor = new int[] { 200, Screen.width / 2, Screen.width - 200 };
    }


    void Update()
    {
        //指が1本触れているとき
        if(Input.touchCount == 1)
        {
            //何も持っていないときは、履歴オブジェクトをつかむ
            if(catching_object == null)
            {

            }
            //履歴オブジェクトを持っているときは移動させる
            //履歴オブジェクトを貼り付けている親オブジェクトの大きさが可変なので、
            else
            {

            }
        }
    }


    //入力履歴に追加
    public void AddToInputHistory(TaskData taskData)
    {
        //現在の履歴オブジェクトの数を取得
        //生成地点のオブジェクトがあるため-1している
        int objectCount = spawn_origin_input_history.transform.parent.childCount - 1;
        //そこから行と列を割り出す
        //上から何番目なのか
        int vertical = objectCount / 3;
        //左から何番目なのか
        int horizontal = objectCount % 3;

        GameObject obje = Instantiate(history_object, spawn_origin_input_history.GetComponent<RectTransform>().position, Quaternion.identity);
        RectTransform objeRect = obje.GetComponent<RectTransform>();
        Vector2 spawnPos = spawn_origin_input_history.GetComponent<RectTransform>().position;
        spawnPos.x = dist_hor[horizontal];
        spawnPos.y -= vertical * dist_ver;
        objeRect.position = spawnPos;
        objeRect.SetParent(input_history_panel_rect);
        objeRect.transform.localScale = Vector3.one;
        obje.GetComponent<HistoryObject>().task_data = taskData;
        obje.GetComponent<HistoryObject>().application_user = application_user;
        obje.transform.GetChild(0).GetComponent<RawImage>().texture = taskData.texture2D;
        if(taskData.mode == true){
            obje.transform.GetChild(0).GetComponent<RawImage>().gameObject.transform.rotation = Quaternion.Euler(0,0,-90);
        }else
        {
            obje.transform.GetChild(0).GetComponent<RawImage>().gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        }
        if(vertical + 1 <= 5)
        {
            
            Vector2 delta = input_history_panel_rect.sizeDelta;
            delta.y = 2810;
            input_history_panel_rect.sizeDelta = delta;
        }
        else
        {
            Vector2 delta = input_history_panel_rect.sizeDelta;
            delta.y = 520 * (vertical + 1);
            input_history_panel_rect.sizeDelta = delta;
        }
    }

    //入力履歴から削除
    public void RemoveFromInputHistory(TaskData taskData)
    {
        RectTransform content_rect = spawn_origin_input_history.GetComponent<RectTransform>().parent.GetComponent<RectTransform>();
        TaskData targetData = null;
        foreach(HistoryObject target in content_rect.GetComponentsInChildren<HistoryObject>())
        {
            if (target.task_data.task_name == taskData.task_name)
            {
                targetData = target.task_data;
                Destroy(target.gameObject);
            }
        }
    }

    //破壊履歴に追加
    public void AddToDestroyHistory(TaskData taskData)
    {
        int objectCount = spawn_origin_destroy_history.transform.parent.childCount - 1;
        //上から何番目なのか
        int vertical = objectCount / 3;
        //左から何番目なのか
        int horizontal = objectCount % 3;

        GameObject obje = Instantiate(history_object, spawn_origin_destroy_history.GetComponent<RectTransform>().position, Quaternion.identity);
        RectTransform objeRect = obje.GetComponent<RectTransform>();
        Vector2 spawnPos = spawn_origin_destroy_history.GetComponent<RectTransform>().position;
        spawnPos.x = dist_hor[horizontal];
        spawnPos.y -= vertical * dist_ver;
        objeRect.position = spawnPos;
        objeRect.SetParent(destroy_history_panel_rect);
        objeRect.transform.localScale = Vector3.one;
        obje.GetComponent<HistoryObject>().task_data = taskData;
        obje.GetComponent<HistoryObject>().application_user = application_user;
        obje.transform.GetChild(0).GetComponent<RawImage>().texture = taskData.texture2D;
        if(taskData.mode == true){
            obje.transform.GetChild(0).GetComponent<RawImage>().gameObject.transform.rotation = Quaternion.Euler(0,0,-90);
        }else
        {
            obje.transform.GetChild(0).GetComponent<RawImage>().gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        }

        if (vertical + 1 <= 5)
        {

            Vector2 delta = destroy_history_panel_rect.sizeDelta;
            delta.y = 2810;
            destroy_history_panel_rect.sizeDelta = delta;
        }
        else
        {
            Vector2 delta = destroy_history_panel_rect.sizeDelta;
            delta.y = 520 * (vertical + 1);
            destroy_history_panel_rect.sizeDelta = delta;
        }
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
    }

    //総合履歴に追加
    public void AddToTotalHisttory(TaskData taskData)
    {
        int objectCount = spawn_origin_total_history.transform.parent.childCount - 1;
        //上から何番目なのか
        int vertical = objectCount / 3;
        //左から何番目なのか
        int horizontal = objectCount % 3;

        GameObject obje = Instantiate(history_object, spawn_origin_total_history.GetComponent<RectTransform>().position, Quaternion.identity);
        RectTransform objeRect = obje.GetComponent<RectTransform>();
        Vector2 spawnPos = spawn_origin_total_history.GetComponent<RectTransform>().position;
        spawnPos.x = dist_hor[horizontal];
        spawnPos.y -= vertical * dist_ver;
        objeRect.position = spawnPos;
        objeRect.SetParent(total_history_panel_rect);
        objeRect.transform.localScale = Vector3.one;
        obje.GetComponent<HistoryObject>().task_data = taskData;
        obje.GetComponent<HistoryObject>().application_user = application_user;
        obje.transform.GetChild(0).GetComponent<RawImage>().texture = taskData.texture2D;
        if(taskData.mode == true){
            obje.transform.GetChild(0).GetComponent<RawImage>().gameObject.transform.rotation = Quaternion.Euler(0,0,-90);
        }else
        {
            obje.transform.GetChild(0).GetComponent<RawImage>().gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        }

        if (vertical + 1 <= 5)
        {

            Vector2 delta = total_history_panel_rect.sizeDelta;
            delta.y = 2810;
            total_history_panel_rect.sizeDelta = delta;
        }
        else
        {
            Vector2 delta = total_history_panel_rect.sizeDelta;
            delta.y = 520 * (vertical + 1);
            total_history_panel_rect.sizeDelta = delta;
        }
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
    }
}
