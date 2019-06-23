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
    private Mode mode;
    //破壊するタスクリストを入れるリスト
    //これでエラーが発生したら、選択したかどうかのフラグをタスクに持たせて破壊する方法に切り替え
    private List<TaskObject> task_list_to_destroy;
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

    void Start()
    {
        taskInputManager = GameObject.Find("TaskInputManager").GetComponent<TaskInputManager>();
        isCatching = false;
        isJudging = false;
    }



    void Update()
    {
        if(Input.touchCount == 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, layerMask))
            {
                if(hit.collider.gameObject.tag == "TaskObject")
                {
                    if(isJudging == false)
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
                yield break;
            }
            if(Input.touchCount == 0)
            {
                isTouching = false;
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
            TaskData taskData = taskObject.GetComponent<TaskData>();
            detail_popup.SetActive(true);
            detail_popup.GetComponent<PopUpObject>().Show(taskData);
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
        else
        {
            AddToDestroyList(taskObject.GetComponent<TaskObject>());
            taskObject.GetComponent<TaskObject>().OnTapToHighlight();
        }
    }

    //破壊リストへ登録する
    private void AddToDestroyList(TaskObject taskObject)
    {
        task_list_to_destroy.Add(taskObject);
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
    public void ChangeToDestroyMode()
    {
        if (mode == Mode.Normal)
            mode = Mode.Destroy;
        else
            mode = Mode.Normal;
    }

    //タスク追加ボタンを押したときに呼ぶメソッド
    public void OnClickAddTaskButton()
    {

    }

    //タスクデータを持たせてタスクオブジェクトを生成する
    public void InstantiateTaskObject(TaskData taskData)
    {
        //Debug.Log("タスクオブジェクト生成");
        if(task_object != null)
        {
            GameObject taskObject = Instantiate(task_object.gameObject, task_spawn_origin.transform.position, Quaternion.identity);
            taskObject.GetComponent<TaskObject>().task_data = taskData;
            taskObject.transform.GetComponentInChildren<UnityEngine.UI.RawImage>().texture = taskData.texture2D;
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
