using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//タスクデータクラスのルート
//JSONファイルに保存するときに使う
public class TaskRoot
{
    public List<TaskData> task_datas;
    public TaskRoot()
    {
        task_datas = new List<TaskData>();
    }
    // public TaskRoot(TaskData[] taskDatas){
    //     task_datas = new List<TaskData>(taskDatas);
    // }
}

// [System.Serializable]
// public class HistoryRoot
// {
//     public List<TaskData> history_datas;
//     public HistoryRoot()
//     {

//     }
// }


[System.Serializable]
//タスクデータクラス
public class TaskData
{
    public string task_name;
    public string task_limit;
    public int task_important_level;
    // public int task_index;
    public bool mode;
    [System.NonSerialized]
    public Texture2D texture2D;
}

public class TaskObject : MonoBehaviour
{
    //タスクデータ
    // [HideInInspector]
    public TaskData task_data;
    //破壊エフェクトのゲームオブジェクト
    [SerializeField]
    private GameObject destroy_effect;
    //破壊サウンド
    [SerializeField]
    private AudioClip destroy_sound;
    //音のスピーカー
    //[SerializeField]
    private AudioSource se_player;
    private HistoryManager history_manager;
    [SerializeField]
    private Material color_blue;
    [SerializeField]
    private Material color_yellow;
    [SerializeField]
    private Material color_red;
    
    void Start()
    {
        se_player = GetComponent<AudioSource>();
        history_manager = GameObject.Find("HistoryManager").GetComponent<HistoryManager>();
    }

    void Update()
    {

    }


    //現在の日付と期限の日付を比較し、
    public void ChangeColor()
    {
        System.DateTime now = System.DateTime.Now; //(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 0, 0, 0);
        System.DateTime nowFixed = System.DateTime.Parse("" + now.Year + "/" + now.Month + "/" + now.Day + " 00:00:00");
        System.DateTime limit = System.DateTime.Parse(task_data.task_limit.Split('_')[0]);

        System.TimeSpan timeSpan = limit - nowFixed;

        if(0 <= timeSpan.TotalDays && timeSpan.TotalDays <= 1)
        {
            GetComponent<Renderer>().sharedMaterial = color_red;
        }
        else if(1 < timeSpan.TotalDays && timeSpan.TotalDays <= 3)
        {
            GetComponent<Renderer>().sharedMaterial = color_yellow;
        }
        else if(timeSpan.TotalDays > 3)
        {
            GetComponent<Renderer>().sharedMaterial = color_blue;
        }

    }


    //破壊モードでタップされたときにハイライトする
    //ハイライトの方法は検討中
    public void OnTapToHighlight()
    {
        GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }

    public void OnTapToCancelHighlight()
    {
        GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
    }

    //破壊されるときにエフェクトと音を鳴らす
    public IEnumerator CallOnDisable()
    {
        history_manager.AddToDestroyHistory(task_data);
        history_manager.RemoveFromInputHistory(task_data);

        if(destroy_effect != null && se_player != null)
        {
            destroy_effect.SetActive(true);
            ParticleSystem particleSystem = destroy_effect.GetComponent<ParticleSystem>();
            se_player.PlayOneShot(destroy_sound);
            if (particleSystem.isPlaying == false)
                particleSystem.Play();
            while (particleSystem.isPlaying || se_player.isPlaying) yield return null;
        }
        Destroy(gameObject);
    }
}
