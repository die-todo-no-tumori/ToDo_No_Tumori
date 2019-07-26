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
}

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
    [SerializeField]
    private AudioClip bound_sound;
    //音のスピーカー
    private AudioSource se_player;
    private HistoryManager history_manager;
    [SerializeField,Header("青色ノーマル")]
    private Material color_blue;

    [SerializeField,Header("青色ハイライト")]
    private Material color_blue_emit;
    [SerializeField,Header("黄色ノーマル")]
    private Material color_yellow;
    [SerializeField,Header("黄色ハイライト")]
    private Material color_yellow_emit;
    [SerializeField,Header("赤ノーマル")]
    private Material color_red;
    [SerializeField,Header("赤ハイライト")]
    private Material color_red_emit;
    
    void Start()
    {
        se_player = Camera.main.gameObject.GetComponent<AudioSource>();
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
        switch(GetComponent<Renderer>().sharedMaterial.name){
            case "TaskBallBlue":
                GetComponent<Renderer>().sharedMaterial = color_blue_emit;
            break;
            case "TaskBallYellow":
                GetComponent<Renderer>().sharedMaterial = color_yellow_emit;
            break;
            case "TaskBallRed":
                GetComponent<Renderer>().sharedMaterial = color_red_emit;
            break;
            default:
            break;
        }
    }

    public void OnTapToCancelHighlight()
    {
        switch(GetComponent<Renderer>().sharedMaterial.name){
            case "TaskBallBlue_Emit":
                GetComponent<Renderer>().sharedMaterial = color_blue;
            break;
            case "TaskBallYellow_Emit":
                GetComponent<Renderer>().sharedMaterial = color_yellow;
            break;
            case "TaskBallRed_Emit":
                GetComponent<Renderer>().sharedMaterial = color_red;
            break;
            default:
            break;
        }
    }

    //破壊されるときにエフェクトと音を鳴らす
    public IEnumerator CallOnDisable()
    {
        TaskData data = new TaskData();
        data = task_data;
        history_manager.AddToDestroyHistory(data);
        history_manager.RemoveFromInputHistory(task_data);

        if(destroy_effect != null && se_player != null)
        {
            destroy_effect.SetActive(true);
            ParticleSystem particleSystem = destroy_effect.GetComponent<ParticleSystem>();
            if(destroy_sound != null)
            	se_player.PlayOneShot(destroy_sound);
            GetComponent<MeshRenderer>().enabled = false;
            //if (particleSystem.isPlaying == false)
                particleSystem.Play();
            while (particleSystem.isPlaying || se_player.isPlaying) yield return null;
        }
        Destroy(gameObject);
    }

    private void OnCollisionExit(Collision other) {
        if(other.gameObject.tag != "Dodai"){
            se_player.PlayOneShot(bound_sound);
        }
    }


}
