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
    
    void Start()
    {
        //if(destroy_effect != null)
        //    destroy_effect.SetActive(false);
        //ChangeColor();
        se_player = GetComponent<AudioSource>();
        history_manager = GameObject.Find("HistoryManager").GetComponent<HistoryManager>();
    }

    void Update()
    {
        //transform.LookAt(Camera.main.transform);
        //transform.Rotate(0, 180, 0);
    }

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
    //public TaskData OnTap()
    //{
    //    return task_data;
    //}

    //破壊モードでタップされたときにハイライトする
    //ハイライトの方法は検討中
    public void OnTapToHighlight()
    {
        GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
    }

    public void OnTapToCancelHighlight()
    {
        GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
    }

    //private void OnDisable()
    //{
    //    StartCoroutine(CallOnDisable());
    //}

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

    //public void OnCollisionEnter(Collision collision)
    //{
    //    //if (collision.gameObject.name == "Dodai 1")
    //    //{
    //        //Debug.Log("enter");
    //    //}
    //}
    //public void OnCollisionExit(Collision collision)
    //{
    //    //if (collision.gameObject.name == "Dodai 1")
    //    //{
    //        //Debug.Log("exit");
    //    //}
    //}
}
