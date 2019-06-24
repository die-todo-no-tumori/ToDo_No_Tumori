using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
    [SerializeField]
    private Button hammer_button;
    //破壊モードのときと通常モードのときのボタンの画像
    [SerializeField]
    private Sprite[] hammer_button_sprites;

    //タスクの詳細を表示する吹き出し
    [SerializeField]
    private GameObject detail_popup;
    [SerializeField]
    private TaskObject task_object;
    [SerializeField]
    private GameObject history_panel;
    [SerializeField]
    private GameObject config_panel;
    [SerializeField]
    private GameObject task_spawn_origin;
    //タスクオブジェクトをつかむのに必要な時間
    [SerializeField]
    private float catch_object_time;
    //つかんでいるタスクオブジェクト
    private GameObject catching_object;
    private bool isCatching;
    [SerializeField]
    private LayerMask layerMask;
    private bool isJudging;

    private TaskInputManager taskInputManager;
    [SerializeField]
    private HistoryManager history_manager;
    [SerializeField]
    private Color camera_normal_color;
    [SerializeField]
    private Color camera_destroy_color;
    [SerializeField]
    private Color light_normal_color;
    [SerializeField]
    private Color light_destroy_color;
    [SerializeField]
    private Light world_light;

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
        if (Input.touchCount == 0)
            catching_object = null;
        if(catching_object != null && isCatching)
        {
            if(Input.touchCount == 0)
            {
                Vector3 posi = catching_object.transform.position;
                posi.z = task_spawn_origin.transform.position.z;
                catching_object.transform.position = posi;
                catching_object = null;
                isCatching = false;
                
            }
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            pos.z = task_spawn_origin.transform.position.z - 1;
            catching_object.transform.position = pos;
        }
        if(Input.touchCount == 1)
        {
            if (isCatching)
                return;
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,100, layerMask))
            {
                if(hit.collider.gameObject.tag == "TaskObject")
                {
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
            if(time > catch_object_time)
            {
                isCatching = true;
                isJudging = false;
                catching_object = obje;
                yield break;
            }
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
        //現在、表示可能な内容は、期限・重要度ぐらいだけど、それは一覧でわかることなので、吹き出しは
        //不要なのでは？
        if(mode == Mode.Normal)
        {
            Vector2 popPos = Camera.main.WorldToViewportPoint(taskObject.transform.position);
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

                }
                //画面左上辺りにある場合
                else if (1480 <= popPos.y && popPos.y <= 2960)
                {
                    //右下に表示

                }
            }else if(720 <= popPos.x && popPos.x <= 1440)
            {
                //画面右下辺りにある場合
                if (0 <= popPos.y && popPos.y < 1480)
                {
                    //左上に表示

                }
                //画面右上辺りにある場合
                else if (1480 <= popPos.y && popPos.y <= 2960)
                {
                    //左下に表示

                }
            }
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
                //Debug.Log("remove from destroy list : " + task_list_to_destroy.Count);
                taskObject.GetComponent<TaskObject>().OnTapToCancelHighlight();
            }
            else
            {
                //破壊リストへ登録されていない場合は、破壊リストへの登録
                task_list_to_destroy.Add(taskObject.GetComponent<TaskObject>());
                //Debug.Log("add to destroy list : " + task_list_to_destroy.Count);
                taskObject.GetComponent<TaskObject>().OnTapToHighlight();
            }
            
        }
    }


    //破壊リストに登録されているタスクオブジェクトを破壊する
    private void DestroyTaskObject(List<TaskObject> taskObjects)
    {
        foreach(TaskObject taskObject in taskObjects)
        {
            Destroy(taskObject.gameObject);
        }
    }

    //履歴画面を表示する
    public void ShowHistory()
    {
        history_panel.SetActive(true);
    }

    //設定画面を表示する
    public void ShowConfig()
    {
        config_panel.SetActive(true);
    }

    //モードを移行する
    public void SwitchMode()
    {
        if (mode == Mode.Normal)
        {
            mode = Mode.Destroy;
            Camera.main.backgroundColor = camera_destroy_color;
            world_light.color = light_destroy_color;
        }
        else
        {
            mode = Mode.Normal;
            Camera.main.backgroundColor = camera_normal_color;
            world_light.color = light_normal_color;
        }
        //ハンマー画像の入れ替え
        hammer_button.image.sprite = hammer_button_sprites[(int)mode];
    }

    //タスク追加ボタンを押したときに呼ぶメソッド
    //public void OnClickAddTaskButton()
    //{

    //}

    //タスクデータを持たせてタスクオブジェクトを生成する
    public void InstantiateTaskObject(TaskData taskData)
    {
        //Debug.Log("タスクオブジェクト生成");
        if(task_object != null)
        {
            GameObject taskObject = Instantiate(task_object.gameObject, task_spawn_origin.transform.position, Quaternion.identity);
            taskObject.GetComponent<TaskObject>().task_data = taskData;
            taskObject.transform.GetComponentInChildren<UnityEngine.UI.RawImage>().texture = taskData.texture2D;
            history_manager.AddToInputHistory(taskData);
            //history_manager.AddToTotalHisttory(taskData);
        }
    }

    public void CreateTask(bool mode)
    {
        StartCoroutine(CreateTaskCor(mode));
    }

    public IEnumerator CreateTaskCor(bool mode)
    {
        TaskData taskData = null;
        yield return StartCoroutine(taskInputManager.MakeTask(data => taskData = data,mode));
        if(taskData != null)
            InstantiateTaskObject(taskData);
    }

}
