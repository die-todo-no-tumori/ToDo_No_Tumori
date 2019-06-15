using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//タスクデータクラスのルート
//JSONファイルに保存するときに使う
public class TaskRoot
{
    public List<TaskData> task_datas;
}


[System.Serializable]
//タスクデータクラス
public class TaskData
{
    public string task_name;
    public string task_limit;
    public byte task_important_level;
    public int task_index;
}

public class TaskObject : MonoBehaviour
{
    //タスクデータ
    [HideInInspector] public TaskData task_data;
    //破壊エフェクトのゲームオブジェクト
    [SerializeField] private GameObject destroy_effect;
    //破壊サウンド
    [SerializeField] private AudioClip destroy_sound;
    //音のスピーカー
    [SerializeField] private AudioSource se_player;
    


    void Start()
    {
        if(destroy_effect != null)
            destroy_effect.SetActive(false);

    }




    void Update()
    {
        
    }

    //日付が変わったかどうかは別のスクリプトで計算させることにしたので、今は不要→コメント化
    //private int GetLimit(TaskData taskData)
    //{
    //    string[] time_data = taskData.task_limit.Split(':');
    //    int month_limit = int.Parse(time_data[0]);
    //    int day_limit = int.Parse(time_data[1]);
        
    //    int month = System.DateTime.Now.Month;
    //    int day = System.DateTime.Now.Day;

    //    return day_limit - day;// + (month_limit - month) * 

    //    //int hour = System.DateTime.Now.Hour;
    //    //int minu = System.DateTime.Now.Minute;
    //    //int sec = System.DateTime.Now.Second;
    //}

    //現在の日付と期限の日付を比較し、
    public void ChangeColor()
    {
        System.DateTime now = System.DateTime.Now; //(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 0, 0, 0);
        System.DateTime limit = System.DateTime.Parse(task_data.task_limit);

        System.TimeSpan timeSpan = limit - now;
        
        if(timeSpan.Days == 1)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }else if(timeSpan.Days == 2 || timeSpan.Days == 3)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if(timeSpan.Days > 3)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }

    }

    //タップされたときにタスクデータを返す
    public TaskData OnTap()
    {
        return this.task_data;
    }

    //破壊モードでタップされたときにハイライトする
    //ハイライトの方法は検討中
    public void OnTapToHighlight()
    {

    }

    public void OnInstantiate()
    {
        ChangeColor();
    }



    private void OnDisable()
    {
        StartCoroutine(CallOnDisable());
    }

    //破壊されるときにエフェクトと音を鳴らす
    private IEnumerator CallOnDisable()
    {
        if(destroy_effect != null)
        {
            destroy_effect.SetActive(true);
            ParticleSystem particleSystem = destroy_effect.GetComponent<ParticleSystem>();
            se_player.PlayOneShot(destroy_sound);
            while (particleSystem.isPlaying || se_player.isPlaying) yield return null;
        }
        yield break;
    }
}
