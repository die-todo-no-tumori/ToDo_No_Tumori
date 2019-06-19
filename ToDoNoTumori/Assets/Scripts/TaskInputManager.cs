using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public enum TaskCreationPhase
{
    Idle = 0,
    Picture,
    ImportantLevel,
    Limit,
    Create
}

public class TaskInputManager : MonoBehaviour
{
    [HideInInspector]
    public Texture2D add_task_image;
    private string add_task_name;
    private string add_task_limit;
    private int? add_task_important_level;
    private bool taked_picture;
    private bool completed_take_picture;
    private bool decided_important_level;
    private bool cancel_decided_important_level;
    private bool decide_task_limit;
    [HideInInspector]
    public bool picture_mode;
    [HideInInspector]
    public static bool movePhase;
    [HideInInspector]
    public static TaskCreationPhase taskCreationPhase;
    [SerializeField]
    private int[] important_level_distance;
    [SerializeField]
    private Vector3[] change_scales;
    [SerializeField]
    private RectTransform important_level_image_rect;
    [SerializeField]
    private Text important_level_text;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private RawImage display_take;
    [SerializeField]
    private RawImage display_choice;
    [SerializeField]
    private RawImage display_to_change_task_level;
    [HideInInspector]
    public Camera main_camera;

    private WebCamTexture webCamTexture;


    IEnumerator Start()
    {
        main_camera = Camera.main;
        //yield break;
        if (WebCamTexture.devices.Length == 0)
        {
            yield break;
        }

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            yield break;
        }

        WebCamDevice userCameraDevice = WebCamTexture.devices[0];
        webCamTexture = new WebCamTexture(userCameraDevice.name, width, height);
        display_take.texture = webCamTexture;
        display_take.transform.rotation = display_take.transform.rotation * Quaternion.AngleAxis(webCamTexture.videoRotationAngle, Vector3.up);
        webCamTexture.Stop();
        taked_picture = false;
        completed_take_picture = false;
        taskCreationPhase = TaskCreationPhase.Idle;
        movePhase = false;
        decided_important_level = false;
        picture_mode = false;
        cancel_decided_important_level = false;
        
        //画像を保存するフォルダの作成
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/Images");
        if (directoryInfo.Exists == false)
            directoryInfo.Create();
        //DontDestroyOnLoad(gameObject);
        
    }

    void Update()
    {
        
    }

    

    public static void GoToNextPhase()
    {
        taskCreationPhase++;
        movePhase = true;
    }

    public static void BackToBeforePhase()
    {
        taskCreationPhase--;
        movePhase = true;
    }


    public void CreateTask(bool mode)
    {
        StartCoroutine(CreateTaskCor(mode));
    }

    public IEnumerator CreateTaskCor(bool mode)
    {
        TaskData taskData = null;
        yield return StartCoroutine(MakeTask(data => taskData = data,mode));
    }

    

    //タスクデータを入力し、タスクデータを返す
    //mode : true -> 撮影 false -> カメラロール
    private IEnumerator MakeTask(UnityAction<TaskData> callBack,bool mode)
    {
        TaskData taskData = new TaskData();
        taskCreationPhase = TaskCreationPhase.Idle;
        picture_mode = mode;
        
        while (true)
        {
            movePhase = false;
            switch (taskCreationPhase)
            {
                case TaskCreationPhase.Picture:
                    add_task_name = "";
                    //タスクの画像を撮影
                    if(picture_mode == true)
                    {
                        StartToTakePicture();
                        yield return StartCoroutine(GetCameraTexture(data => add_task_image = data));
                        display_take.texture = add_task_image;
                    }
                    else
                    {
                        yield return StartCoroutine(GetCameraRollTexture(data => add_task_image = data));
                    }
                    break;
                case TaskCreationPhase.ImportantLevel:
                    //タスクの重要度を決める
                    decided_important_level = false;
                    yield return StartCoroutine(ChangeTaskImportantLevel(data => add_task_important_level = data));
                    break;
                case TaskCreationPhase.Limit:
                    //タスクの期限を決める
                    decide_task_limit = false;
                    yield return StartCoroutine(SetLimitOfTask(data => add_task_limit = data));
                    break;
                case TaskCreationPhase.Create:
                    //タスクデータを保存する
                    
                    yield break;
                default:
                    break;
            }
            while (movePhase == false)
                yield return null;
        }
    }

    //カメラで撮影したデータをテクスチャにして返す
    private IEnumerator GetCameraTexture(UnityAction<Texture2D> callBack)
    {
        GameObject.Find("TakePictureButton").transform.GetChild(0).GetComponent<Text>().text = "撮影";
        //写真が取られるまで待機
        while (taked_picture == false)
        {
            yield return null;
            if(taskCreationPhase != TaskCreationPhase.Picture)
            {
                callBack(null);
                yield break;
            }
        }
        PauseToTakePicture();
        //GameObject.Find("TakePictureButton").transform.GetChild(0).GetComponent<Text>().text = "保存中";
        Texture2D texture2D = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.ARGB32, false);
        texture2D.SetPixels(webCamTexture.GetPixels());
        texture2D.Apply();
        //byte[] data = texture2D.EncodeToPNG();
        add_task_name = System.DateTime.Now.ToLongDateString() + "," + System.DateTime.Now.ToLongTimeString();
        //File.WriteAllBytes(Application.persistentDataPath + "/Images/" + dateData +".png", data);
        yield return new WaitForEndOfFrame();
        GameObject.Find("TakePictureButton").transform.GetChild(0).GetComponent<Text>().text = "撮影完了";
        //カメラを止める
        webCamTexture.Stop();
        callBack(texture2D);
    }


    //カメラロールから写真データを取得して返す
    private IEnumerator GetCameraRollTexture(UnityAction<Texture2D> callBack)
    {
        while(display_choice.texture == null)
        {
            yield return null;
            if(taskCreationPhase != TaskCreationPhase.Picture)
            {
                callBack(null);
                yield break;
            }
        }
        Texture2D texture2D = ToTexture2D(display_choice.texture);
        yield return new WaitForEndOfFrame();
        callBack(texture2D);
    }

    //TextureとTexture2Dを変換
    public Texture2D ToTexture2D(Texture self)
    {
        int sw = self.width;
        int sh = self.height;
        Texture2D result = new Texture2D(sw, sh, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture rt = new RenderTexture(sw, sh, 32);
        Graphics.Blit(self, rt);
        RenderTexture.active = rt;
        Rect source = new Rect(0, 0, rt.width, rt.height);
        result.ReadPixels(source, 0, 0);
        result.Apply();
        RenderTexture.active = currentRT;
        return result;
    }

    //ピンチインとピンチアウトでタスクオブジェクトの重要度を変える
    private IEnumerator ChangeTaskImportantLevel(UnityAction<int> callBack)
    {
        int level = 0;
        display_to_change_task_level.texture = add_task_image;
        while(decided_important_level == false)
        {
            //2本指で触れているときのみ判定
            if(Input.touchCount == 2)
            {
                Touch[] touches = Input.touches;
                //タッチの状態により距離を計測
                if(touches[0].phase == TouchPhase.Began || touches[0].phase == TouchPhase.Moved || touches[0].phase == TouchPhase.Stationary
                    || touches[1].phase == TouchPhase.Began || touches[1].phase == TouchPhase.Moved || touches[1].phase == TouchPhase.Stationary)
                {
                    Vector2 vect = touches[0].position - touches[1].position;
                    float dist = vect.magnitude;
                    //重要度１
                    if(important_level_distance[0] <= dist && dist <= (important_level_distance[0] + important_level_distance[1]) / 2)
                    {
                        level = 0;
                    }
                    //重要度２
                    else if ((important_level_distance[0] + important_level_distance[1]) / 2 <= dist && dist <= (important_level_distance[1] + important_level_distance[2]) / 2)
                    {
                        level = 1;
                    }
                    //重要度３
                    else if((important_level_distance[1] + important_level_distance[2]) / 2 <= dist && dist <= important_level_distance[2])
                    {
                        level = 2;
                    }
                    important_level_text.text = "重要度:" + (level + 1);
                    important_level_image_rect.localScale = change_scales[level];
                }
            }
            if (cancel_decided_important_level == true)
            {
                //Debug.Log("キャンセル");
                yield break;
            }

            yield return null;
        }
        yield return null;
        callBack(level);
    }

    //カレンダーに投げて、タスクの期限を設定する
    private IEnumerator SetLimitOfTask(UnityAction<string> callBack)
    {
        while (decide_task_limit == false)
            yield return null;

        if(add_task_limit == "" || add_task_limit == null)
        {
            yield break;
        }
    }

    //タスク追加操作をキャンセルする
    private void CancelToAddTask()
    {
        //RawImageのテクスチャを消す
        //display_take = null;
        display_choice = null;
        Destroy(add_task_image);
        //期限と重要度を消す
        add_task_important_level = 0;
        add_task_limit = null;
        webCamTexture.Stop();
    }

    //通知に登録する
    private void AddTaskToNotice()
    {

    }

    //撮影開始
    public void StartToTakePicture()
    {
        webCamTexture.Play();
        display_take.texture = webCamTexture;
        taked_picture = false;
        completed_take_picture = false;
        //Debug.Log("撮影スタート");
    }

    //取り直し
    public void RestartToTakePicture()
    {
        movePhase = true;
    }

    //撮影
    public void TakePicture()
    {
        taked_picture = true;
    }

    //重要度決定
    public void DecideImportantLevel()
    {
        decided_important_level = true;
    }

    //重要度決定キャンセル
    public void CancelDecideImportantLevel()
    {
        cancel_decided_important_level = true;
    }

    public void OpenLimitScene(string name)
    {
        StartCoroutine(OpenLimitScene());
    }

    private IEnumerator OpenLimitScene()
    {
        main_camera.gameObject.SetActive(false);
        yield return SceneManager.LoadSceneAsync("Calender", LoadSceneMode.Additive);
    }

    //タスクの期限決定
    public void DecideTaskLimit()
    {
        decide_task_limit = true;
    }

    //タスク期限設定
    public void SetAddTaskLimit(string limit)
    {
        add_task_limit = limit;
        main_camera.gameObject.SetActive(true);
    }

    //撮影一時停止
    public void PauseToTakePicture()
    {
        if (webCamTexture.isPlaying == false) return;
        webCamTexture.Pause();
    }

    //撮影停止
    public void StopToTakePicture()
    {
        if (webCamTexture.isPlaying)
            webCamTexture.Stop();
    }

}
