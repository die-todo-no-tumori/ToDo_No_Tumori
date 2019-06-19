using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HammerMode
{
    Normal = 0,
    Destroy
}


public class ApplicationUser : MonoBehaviour
{
    private HammerMode hammer_mode;
    //破壊するタスクリストを入れるリスト
    //これでエラーが発生したら、選択したかどうかのフラグをタスクに持たせて破壊する方法に切り替え
    private List<TaskObject> task_list_to_destroy;
    //タスクの詳細を表示する吹き出し
    //[SerializeField]
    //private GameObject task_info_bubble;
    private TaskObject task_object;
    [SerializeField]
    private GameObject detail_popup;

    void Start()
    {
        
    }



    void Update()
    {
        
    }

    //オブジェクトをタップしたときに、モードにより違う処理を行う
    private void Tap(GameObject taskObject)
    {
        //通常モード
        //吹き出しを出すだけ
        if(hammer_mode == HammerMode.Normal)
        {

        }
        //破壊モード
        //タップしたオブジェクトを破壊リストに登録する
        else
        {

        }
    }

    //破壊リストへ登録する
    private void AddToDestroyList(TaskObject taskObject)
    {

    }

    //破壊リストに登録されているタスクオブジェクトを破壊する
    private void DestroyTaskObject(List<TaskObject> taskObjects)
    {

    }

    //履歴画面を表示する
    public void ShowHistory()
    {

    }

    //設定画面を表示する
    public void ShowConfig()
    {

    }

    //破壊モードへ移行する
    public void ChangeToDestroyMode()
    {

    }

    //タスク追加ボタンを押したときに呼ぶメソッド
    public void OnClickAddTaskButton()
    {

    }

    //タスクデータを持たせてタスクオブジェクトを生成する
    public void InstantiateTaskObject(TaskData taskData)
    {

    }

}
