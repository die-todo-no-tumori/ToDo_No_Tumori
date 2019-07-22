using System.Collections;
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
    [SerializeField,Header("タスクタップ音")]
    private AudioClip task_tap_sound;
    [SerializeField,Header("タスクスロー音")]
    private AudioClip task_throw;
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
    private Vector2 before_before_finger_pos;
    private Rigidbody catching_object_rigid;

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
            if(Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary){
                    if(270 <= touch.position.x && touch.position.x <= 1150){
                        if(before_finger_pos == Vector2.zero){
                            before_finger_pos = touch.position;
                        }else
                        {
                            before_before_finger_pos = before_finger_pos;
                            before_finger_pos = touch.position;
                        }
                    }
                    Vector3 screen_point = Camera.main.WorldToScreenPoint(catching_object.transform.position);
                    Vector3 pos = new Vector3(before_finger_pos.x, before_finger_pos.y, screen_point.z);
                    catching_object_rigid.position = Camera.main.ScreenToWorldPoint(pos);
                }
                //離された瞬間
                else if(touch.phase == TouchPhase.Ended){
                    Vector2 direct = before_finger_pos - before_before_finger_pos;
                    catching_object_rigid.AddForce(direct * flick_power,ForceMode.VelocityChange);
                    catching_object_rigid.useGravity = true;
                    catching_object_rigid = null;
                    catching_object = null;
                    isCatching = false;
                    before_before_finger_pos = Vector2.zero;
                    before_finger_pos = Vector2.zero;
                    se_player.PlayOneShot(task_throw);
                }
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
                    if(mode == Mode.Normal){
                        //タップか長押しかの判定がされていなければ、判定する
                        if (isJudging == false)
                        {
                            isJudging = true;
                            StartCoroutine(JudgeTapOrCatch(hit.collider.gameObject));
                        }
                    }else
                    {
                        if(Input.GetTouch(0).phase == TouchPhase.Began)
                            Tap(hit.collider.gameObject);
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
                catching_object_rigid = obje.GetComponent<Rigidbody>();
                catching_object_rigid.useGravity = false;
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
            se_player.PlayOneShot(task_tap_sound);
            Vector2 popPos = Camera.main.WorldToScreenPoint(taskObject.transform.position);
            TaskData taskData = taskObject.GetComponent<TaskObject>().task_data;
            int fix = 100;
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
                    popPos.x += detail_popup.GetComponent<RectTransform>().rect.width / 2 + fix;
                    popPos.y += detail_popup.GetComponent<RectTransform>().rect.height / 2 + fix;
                }
                //画面左上辺りにある場合
                else if (1480 <= popPos.y && popPos.y <= 2960)
                {
                    //右下に表示
                    popPos.x += detail_popup.GetComponent<RectTransform>().rect.width / 2 + fix;
                    popPos.y -= detail_popup.GetComponent<RectTransform>().rect.height / 2 + fix;
                }
            }else if(720 <= popPos.x && popPos.x <= 1440)
            {
                //画面右下辺りにある場合
                if (0 <= popPos.y && popPos.y < 1480)
                {
                    //左上に表示
                    popPos.x -= detail_popup.GetComponent<RectTransform>().rect.width / 2 + fix;
                    popPos.y += detail_popup.GetComponent<RectTransform>().rect.height / 2 + fix;
                }
                //画面右上辺りにある場合
                else if (1480 <= popPos.y && popPos.y <= 2960)
                {
                    //左下に表示
                    popPos.x -= detail_popup.GetComponent<RectTransform>().rect.width / 2 + fix;
                    popPos.y -= detail_popup.GetComponent<RectTransform>().rect.height / 2 + fix;
                }
            }
            detail_popup.GetComponent<RectTransform>().position = popPos;
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
            history_panel.transform.GetChild(0).gameObject.SetActive(true);
            history_panel.transform.GetChild(1).GetComponent<Button>().enabled = false;
            history_panel.transform.GetChild(2).gameObject.SetActive(false);
            history_panel.transform.GetChild(4).gameObject.SetActive(false);
            
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
            //色を変える
            taskObject.GetComponent<TaskObject>().ChangeColor();
            // save_and_loader.SaveDatas();
            return taskObject;
        }
        return null;
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
            yield return StartCoroutine(task_list_to_destroy[i].CallOnDisable());
        }
        task_list_to_destroy.Clear();
        save_and_loader.SaveDatas();
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
    }
}
