using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;

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
    private Texture2D add_task_image;
    private string add_task_limit;
    private int add_task_important_level;
    private bool taked_picture;
    private bool completed_take_picture;
    [HideInInspector]
    public static bool movePhase;
    [HideInInspector]
    public static TaskCreationPhase taskCreationPhase;
    //[SerializeField]
    //private Button retakePictureButton;

    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private RawImage display_take;
    [SerializeField]
    private RawImage display_choice;

    private WebCamTexture webCamTexture;


    IEnumerator Start()
    {
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
        
        //写真撮り直しボタンの設定のみここで行う
        //retakePictureButton.onClick.AddListener(() =>
        //{
        //    RestartToTakePicture();
        //});

        //画像を保存するフォルダの作成
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/Images");
        if (directoryInfo.Exists == false)
            directoryInfo.Create();
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
        
        //Debug.Log("タスク作成開始");
        while (true)
        {
            movePhase = false;
            switch (taskCreationPhase)
            {
                case TaskCreationPhase.Picture:
                    //タスクの画像を撮影
                    if(mode == true)
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
                    yield return StartCoroutine(ChangeTaskImportantLevel(data => add_task_important_level = data));
                    break;
                case TaskCreationPhase.Limit:
                    //タスクの期限を決める
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
            //Debug.Log("待機");
            yield return null;
            //Debug.Log(taskCreationPhase);
            if(taskCreationPhase != TaskCreationPhase.Picture)
            {
                //Debug.Log("出る");
                callBack(null);
                yield break;
            }
        }
        PauseToTakePicture();
        GameObject.Find("TakePictureButton").transform.GetChild(0).GetComponent<Text>().text = "保存中";
        //display_take.texture = webCamTexture;
        Texture2D texture2D = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.ARGB32, false);
        texture2D.SetPixels(webCamTexture.GetPixels());
        texture2D.Apply();
        //Debug.Log("撮影完了");
        byte[] data = texture2D.EncodeToPNG();
        string dateData = System.DateTime.Now.ToLongDateString() + "," + System.DateTime.Now.ToLongTimeString();
        File.WriteAllBytes(Application.persistentDataPath + "/Images/" + dateData +".png", data);
        //Debug.Log("保存");
        yield return new WaitForEndOfFrame();
        GameObject.Find("TakePictureButton").transform.GetChild(0).GetComponent<Text>().text = "保存完了";
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
        yield return null;
    }

    //カレンダーに投げて、タスクの期限を設定する
    private IEnumerator SetLimitOfTask(UnityAction<string> callBack)
    {
        yield return null;
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
    }

    //通知に登録する
    private void AddTaskToNotice()
    {

    }

    //撮影開始
    public void StartToTakePicture()
    {
        //if (webCamTexture.isPlaying == false)
        webCamTexture.Play();
        display_take.texture = webCamTexture;
        taked_picture = false;
        completed_take_picture = false;
    }

    //取り直し
    public void RestartToTakePicture()
    {
        movePhase = true;
    }

    public void TakePicture()
    {
        taked_picture = true;
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
