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
        destroy_effect.SetActive(false);
    }




    void Update()
    {
        
    }

    //現状は何日前であるかのみを計算する
    //また、現在は同じ月でないといけないのが問題点
    private int GetLimit(TaskData taskData)
    {
        string[] time_data = taskData.task_limit.Split(':');
        int month_limit = int.Parse(time_data[0]);
        int day_limit = int.Parse(time_data[1]);
        
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;

        return day_limit - day;// + (month_limit - month) * 

        //int hour = System.DateTime.Now.Hour;
        //int minu = System.DateTime.Now.Minute;
        //int sec = System.DateTime.Now.Second;
    }

    //1秒ごとに時間を測って、残り時間により色を変える
    //日付の差で色を変えるなら、不要なのでは？
    private IEnumerator ChangeColor()
    {
        while (true)
        {
            int diff = GetLimit(task_data);
            if(0 <= diff && diff <= 1)
            {
                GetComponent<Renderer>().material.color = Color.red;
            }else if(2 <= diff && diff <= 3)
            {
                GetComponent<Renderer>().material.color = Color.yellow;
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.blue;
            }
            yield return new WaitForSeconds(1);
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

    private void OnDisable()
    {
        StartCoroutine(CallOnDisable());
    }

    //破壊されるときにエフェクトと音を鳴らす
    private IEnumerator CallOnDisable()
    {
        destroy_effect.SetActive(true);
        ParticleSystem particleSystem = destroy_effect.GetComponent<ParticleSystem>();
        se_player.PlayOneShot(destroy_sound);
        while (particleSystem.isPlaying || se_player.isPlaying) yield return null;
        yield break;
    }
}
