﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Mode
{
    Normal = 0,
    Destroy
}


public class ApplicationUser : MonoBehaviour
{
    //破壊モードか通常モードか
    private Mode mode;
    //破壊するタスクリストを入れるリスト
    //これでエラーが発生したら、選択したかどうかのフラグをタスクに持たせて破壊する方法に切り替え
    private List<TaskObject> task_list_to_destroy;
    //ハンマーボタン
    [SerializeField,Header("ハンマーボタン")]
    private Button hammer_button;
    //破壊モードのときと通常モードのときのボタンの画像
    [SerializeField,Header("ハンマー画像")]
    private Sprite[] hammer_button_sprites;

    //タスクの詳細を表示する吹き出し
    [SerializeField,Header("ポップアップボタンオブジェクト")]
    private GameObject detail_popup;
    //生成するタスクオブジェクト
    [SerializeField,Header("生成するタスクオブジェクト")]
    private TaskObject task_object;
    //履歴パネル
    [SerializeField,Header("履歴パネル")]
    private GameObject history_panel;
    //設定パネル
    [SerializeField,Header("設定パネル")]
    private GameObject config_panel;
    //タスクオブジェクト生成地点
    [SerializeField,Header("タスク生成地点")]
    private GameObject task_spawn_origin;
    //タスクオブジェクトをつかむのに必要な時間
    [SerializeField,Header("つかむのにかかる時間")]
    private float catch_object_time;
    //つかんでいるタスクオブジェクト
    private GameObject catching_object;
    private bool isCatching;
    [SerializeField,Header("RayCastのマスク")]
    private LayerMask layerMask;
    private bool isJudging;
    [SerializeField]
    private LayerMask move_panel_layer;

    private TaskInputManager taskInputManager;
    [SerializeField]
    private HistoryManager history_manager;
    //通常時のカメラの背景色
    [SerializeField,Header("カメラ背景色(ノーマル)")]
    private Color camera_normal_color;
    //破壊時のカメラの背景色
    [SerializeField,Header("カメラ背景色(破壊時)")]
    private Color camera_destroy_color;
    //通常時のライトの色
    [SerializeField,Header("ライトの色(ノーマル)")]
    private Color light_normal_color;
    //破壊時のライトの色
    [SerializeField,Header("ライトの色(破壊時)")]
    private Color light_destroy_color;
    //ライトオブジェクト
    [SerializeField,Header("ライトオブジェクト")]
    private Light world_light;
    [SerializeField,Header("SEプレイヤー")]
    private AudioSource se_player;
    [SerializeField,Header("決定音")]
    private AudioClip positive_sound;
    [SerializeField,Header("キャンセル音")]
    private AudioClip negative_sound;
    [SerializeField]
    private GameObject task_menu_button;
    [SerializeField]
    private GameObject task_butto_parent;
    [SerializeField,Header("破壊ボタン")]
    private GameObject break_button;
    [SerializeField]
    private CalenderMaker calender_maker;
    [SerializeField]
    private float[] task_object_scale_per_important_level;
    [SerializeField]
    private SaveAndLoader save_and_loader;
    [SerializeField,Header("タスクオブジェクトを飛ばす力")]
    private float flick_power;
    
    private Vector2 before_finger_pos;

    void Start()
    {
        taskInputManager = GameObject.Find("TaskInputManager").GetComponent<TaskInputManager>();
        isCatching = false;
        isJudging = false;
        mode = Mode.Normal;
        task_list_to_destroy = new List<TaskObject>();
    }



    void Update()
    {
        //タスクオブジェクトを動かす
        if (catching_object != null && isCatching)
        {
            //指が離されたとき
            if (Input.touchCount == 0)
            {
                //落とすのではなく、飛ばす
                Vector2 now_finger_pos = Input.mousePosition;
                Vector2 move_vector = now_finger_pos - before_finger_pos;
                catching_object.GetComponent<Rigidbody>().AddForce(move_vector * flick_power,ForceMode.VelocityChange);
                //Vector3 posi = catching_object.transform.position;
                //posi.z = task_spawn_origin.transform.position.z;
                //catching_object.transform.position = posi;
                catching_object = null;
                isCatching = false;
            }//指が触れている間は指の位置に追尾させる
            else if(Input.touchCount == 1)
            {
            	before_finger_pos = Input.mousePosition;
            	//Vector2 move_vector = now_finger_pos - before_finger_pos;
            	//catching_object.GetComponent<Rigidbody>().AddForce(move_vector,ForceMode.VelocityChange);
                Vector3 screen_point = Camera.main.WorldToScreenPoint(catching_object.transform.position);
                Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screen_point.z);
                catching_object.transform.position = Camera.main.ScreenToWorldPoint(pos);
            }
        }
        //タスクオブジェクトを持つ
        else if (Input.touchCount == 1)
        {
            if (isCatching)
                return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                //タスクオブジェクトに触れた時
                if (hit.collider.gameObject.tag == "TaskObject")
                {
                    //タップか長押しかの判定がされていなければ、判定する
                    if (isJudging == false)
                    {
                        isJudging = true;
                        StartCoroutine(JudgeTapOrCatch(hit.collider.gameObject));
                    }
                }
            }
        }
    }

    //画面をタッチしたときに、タップか長押しかを判定する
    private IEnumerator JudgeTapOrCatch(GameObject obje)
    {
        bool isTouching = true;
        float time = 0;
        while (isTouching)
        {
            time += Time.deltaTime;
            yield return null;
            //触れている時間が長押しの判定を取る時間を超えたときは、タップ
            if(time > catch_object_time)
            {
                isCatching = true;
                isJudging = false;
                catching_object = obje;
                before_finger_pos = Input.mousePosition;
                yield break;
            }
            //設定時間を超えるまでに離されたときは、タップ
            if(Input.touchCount == 0)
            {
                isTouching = false;
                isJudging = false;
                Tap(obje);
            }
        }
    }

    //オブジェクトをタップしたときに、モードにより違う処理を行う
    private void Tap(GameObject taskObject)
    {
        //通常モード
        //吹き出しを出すだけ
        //現在、表示可能な内容は、期限の詳細
        if(mode == Mode.Normal)
        {
            Vector2 popPos = Camera.main.WorldToViewportPoint(taskObject.transform.position);
            Debug.Log(popPos);
            TaskData taskData = taskObject.GetComponent<TaskObject>().task_data;
            if(detail_popup != null)
            {
                detail_popup.SetActive(true);
                detail_popup.GetComponent<PopUpObject>().Show(taskData);
            }
            //タスクオブジェクトの位置に応じて、吹き出しの位置を変える
            if (0 <= popPos.x && popPos.x < 720)
            {
                //画面左下辺りにある場合
                if (0 <= popPos.y && popPos.y < 1480)
                {
                    //右上に表示
                    popPos.x += detail_popup.GetComponent<RectTransform>().rect.width / 2;
                    popPos.y += detail_popup.GetComponent<RectTransform>().rect.height / 2;
                }
                //画面左上辺りにある場合
                else if (1480 <= popPos.y && popPos.y <= 2960)
                {
                    //右下に表示
                    popPos.x += detail_popup.GetComponent<RectTransform>().rect.width / 2;
                    popPos.y -= detail_popup.GetComponent<RectTransform>().rect.height / 2;
                }
            }else if(720 <= popPos.x && popPos.x <= 1440)
            {
                //画面右下辺りにある場合
                if (0 <= popPos.y && popPos.y < 1480)
                {
                    //左上に表示
                    popPos.x -= detail_popup.GetComponent<RectTransform>().rect.width / 2;
                    popPos.y += detail_popup.GetComponent<RectTransform>().rect.height / 2;
                }
                //画面右上辺りにある場合
                else if (1480 <= popPos.y && popPos.y <= 2960)
                {
                    //左下に表示
                    popPos.x -= detail_popup.GetComponent<RectTransform>().rect.width / 2;
                    popPos.y -= detail_popup.GetComponent<RectTransform>().rect.height / 2;
                }
            }
            detail_popup.GetComponent<RectTransform>().localPosition = popPos;
        }
        //破壊モード
        //タップしたオブジェクトを破壊リストに登録する
        //さらに、登録したオブジェクトをハイライトする
        else
        {
            //破壊リストへ登録されている場合は,リストから削除
            if (task_list_to_destroy.Contains(taskObject.GetComponent<TaskObject>()))
            {
                task_list_to_destroy.Remove(taskObject.GetComponent<TaskObject>());
                taskObject.GetComponent<TaskObject>().OnTapToCancelHighlight();
            }
            else
            {
                //破壊リストへ登録されていない場合は、破壊リストへの登録
                task_list_to_destroy.Add(taskObject.GetComponent<TaskObject>());
                taskObject.GetComponent<TaskObject>().OnTapToHighlight();
            }
        }
    }


    //ホーム画面を表示する
    public void ShowHome()
    {
        history_panel.SetActive(false);
        config_panel.SetActive(false);
        se_player.PlayOneShot(positive_sound);
    }

    //履歴画面の表示を切り替える
    public void ShowHistory()
    {
        if (history_panel.activeSelf)
        {
            history_panel.SetActive(false);
            se_player.PlayOneShot(negative_sound);
        }
        else
        {
            history_panel.SetActive(true);
            se_player.PlayOneShot(positive_sound);
            history_panel.transform.GetChild(1).GetComponent<Button>().enabled = false;
        }
        config_panel.SetActive(false);
    }

    //設定画面の表示を切り替える
    public void ShowConfig()
    {
        if (config_panel.activeSelf)
        {
            config_panel.SetActive(false);
            se_player.PlayOneShot(negative_sound);
        }
        else
        {
            config_panel.SetActive(true);
            se_player.PlayOneShot(positive_sound);
        }
        history_panel.SetActive(false);
    }

    //モードを移行する
    public void SwitchMode()
    {
        if (mode == Mode.Normal)
        {
            mode = Mode.Destroy;
            Camera.main.backgroundColor = camera_destroy_color;
            world_light.color = light_destroy_color;
            task_menu_button.SetActive(false);
            task_butto_parent.SetActive(false);
            break_button.SetActive(true);
        }
        else
        {
            mode = Mode.Normal;
            Camera.main.backgroundColor = camera_normal_color;
            world_light.color = light_normal_color;
            task_menu_button.SetActive(true);
            task_butto_parent.SetActive(true);
            break_button.SetActive(false);
        }
        //ハンマー画像の入れ替え
        hammer_button.image.sprite = hammer_button_sprites[(int)mode];
    }


    //タスクデータを持たせてタスクオブジェクトを生成する
    //このタイミングでデータを保存する
    public GameObject InstantiateTaskObject(TaskData taskData, bool mode)
    {
        if(task_object != null)
        {
            GameObject taskObject = Instantiate(task_object.gameObject, task_spawn_origin.transform.position, Quaternion.identity);
            //重要度に応じてタスクの大きさを変化させる
            Vector3 scale = taskObject.transform.localScale;
            scale.x *= task_object_scale_per_important_level[taskData.task_important_level];
            scale.y *= task_object_scale_per_important_level[taskData.task_important_level];
            taskObject.transform.localScale = scale;
            //タスクデータを持たせる
            taskObject.GetComponent<TaskObject>().task_data = taskData;
            //タスクイメージを表示させる
            taskObject.transform.GetComponentInChildren<RawImage>().texture = taskData.texture2D;
            //撮影モードと選択モードで、表示させるRawImageの角度を変える
            taskObject.transform.GetComponentInChildren<RawImage>().transform.rotation = mode ? Quaternion.Euler(0,0,-90):Quaternion.Euler(0,0,0);
            // save_and_loader.SaveDatas();
            return taskObject;
        }
        return null;

        // save_and_loader.SaveDatas();
    }

    //タスクオブジェクトの破壊メソッドs
    public void BreakTaskObjects()
    {
        StartCoroutine(BreakTaskObjectsCor());
    }

    //破壊リストに登録したタスクオジェクトを破壊する
    public IEnumerator BreakTaskObjectsCor()
    {
        for(int i = 0; i < task_list_to_destroy.Count;i++)
        {
            StartCoroutine(task_list_to_destroy[i].CallOnDisable());
            yield return new WaitForSeconds(0.1f);
        }
        task_list_to_destroy.Clear();
    }

    //タスク作成メソッド
    public void CreateTask(bool mode)
    {
        StartCoroutine(CreateTaskCor(mode));
    }

    //タスクの作成を行うコルーチンを呼び出すメソッド
    public IEnumerator CreateTaskCor(bool mode)
    {
        TaskData taskData = null;
        yield return StartCoroutine(taskInputManager.MakeTask(data => taskData = data,mode));
        if(taskData != null){
            InstantiateTaskObject(taskData,mode);
            history_manager.AddToInputHistory(taskData);
            history_manager.AddToTotalHisttory(taskData);
            save_and_loader.SaveDatas();
        }
        Destroy(calender_maker.ball);
    }
}
