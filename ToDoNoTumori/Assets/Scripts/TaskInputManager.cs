﻿using System.Collections;
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
    //保存するタスクの画像イメージ
    // [HideInInspector]
    public Texture2D add_task_image;
    //タスクの名前　名前設定はしないので、画像作成時点の時刻にする
    private string add_task_name;
    //タスクの期限
    [HideInInspector]
    public string add_task_limit;
    //タスクの重要度
    private int? add_task_important_level;
    //写真を撮ったかどうか
    private bool taked_picture;
    //写真を選択したかどうか
    private bool choiced_picture;
    //タスクの重要度を決定したかどうか
    private bool decided_important_level;
    //タスクの重要度設定をキャンセルしたかどうか
    private bool cancel_decided_important_level;
    //タスクの期限を決定したかどうか
    private bool decide_task_limit;
    //タスクの期限設定をキャンセルしたかどうか
    private bool canceled_to_set_task_limit;
    //タスクの追加操作そのものをキャンセルしたかどうか
    private bool canceled_to_add_task;
    //画像選択モード　true -> 撮影　false -> カメラロール
    [HideInInspector]
    public bool picture_mode;
    //タスク作成のフェーズを移行するかどうか
    [HideInInspector]
    public static bool movePhase;
    //タスク作成のフェーズ
    [HideInInspector]
    public static TaskCreationPhase taskCreationPhase;
    //重要度を決めるための指の距離を入れる配列
    [SerializeField]
    private int[] important_level_distance;
    //重要度を変えたときに変化するタスクの画像の大きさの配列
    [SerializeField]
    private Vector3[] change_scales;
    //重要度設定をするときのタスクの画像を貼り付けるRawImageのRectTransform
    [SerializeField]
    private RectTransform important_level_image_rect;
    //重要度を表示するテキスト
    [SerializeField]
    private Text important_level_text;
    //カメラの幅
    [SerializeField]
    private int width;
    //カメラの高さ
    [SerializeField]
    private int height;
    //画像撮影時にカメラのイメージを入れるRawImage
    [SerializeField]
    private RawImage display_take;
    //画像選択時に選択したイメージを入れるRawImage
    [SerializeField]
    private RawImage display_choice;
    //重要度設定時にイメージを入れるRawImage
    [SerializeField]
    private RawImage display_to_change_task_level;
    //カメラ（撮影時に使う端末のカメラ）
    private WebCamTexture webCamTexture;
    //タスク作成画面で邪魔になるボタンのオブジェクト
    [SerializeField]
    private GameObject open_add_menu_button;
    [SerializeField]
    private GameObject button_parent;
    [SerializeField]
    private GameObject hammer_button;
    //期限設定を行うカレンダーの生成スクリプト
    [SerializeField]
    private CalenderMaker calender_maker;

    //パネル
    [SerializeField]
    private GameObject take_picture_panel;
    [SerializeField]
    private GameObject choice_picture_panel;
    [SerializeField]
    private GameObject important_level_panel;
    [SerializeField]
    private GameObject set_task_limit_panel;

    //カメラのレイヤーマスク
    [SerializeField]
    private LayerMask normal_mask;
    [SerializeField]
    private LayerMask task_limit_mask;

    //非表示にしたいボタン
    [SerializeField]
    private GameObject home_button;
    [SerializeField]
    private GameObject history_button;
    [SerializeField]
    private GameObject config_button;


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
        taskCreationPhase = TaskCreationPhase.Idle;
        movePhase = false;
        decided_important_level = false;
        picture_mode = false;
        cancel_decided_important_level = false;
        canceled_to_add_task = false;
        canceled_to_set_task_limit = false;

        //画像を保存するフォルダの作成
        DirectoryInfo directoryInfo = null;// new DirectoryInfo(Application.persistentDataPath + "/Images");
#if UNITY_EDITOR
        directoryInfo = new DirectoryInfo(Application.persistentDataPath + @"\Images");
#elif UNITY_ANDROID
        directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/Images");
#endif
        if (directoryInfo.Exists == false)
            directoryInfo.Create();


    }

    void Update()
    {

    }

    //フェーズを一つ進む
    public void GoToNextPhase()
    {
        taskCreationPhase++;
        movePhase = true;
    }

    //フェーズを一つ戻す
    public void BackToBeforePhase()
    {
        taskCreationPhase--;
        movePhase = true;
    }


    //タスクデータを入力し、タスクデータを返す
    //mode : true -> 撮影 false -> カメラロール
    public IEnumerator MakeTask(UnityAction<TaskData> callBack,bool mode)
    {
        //返す予定のタスクデータクラスのインスタンスを用意
        TaskData taskData = null;
        //タスク作成フェーズを最初の状態にする
        taskCreationPhase = TaskCreationPhase.Idle;
        //写真を撮影するか選択するかのモードを入れる
        picture_mode = mode;

        //不用なボタンを見えなくする
        hammer_button.SetActive(false);
        open_add_menu_button.SetActive(false);
        button_parent.SetActive(false);
        home_button.SetActive(false);
        history_button.SetActive(false);
        config_button.SetActive(false);
        add_task_image = new Texture2D(webCamTexture.width, webCamTexture.height);
        while (true)
        {
            // Debug.Log(taskCreationPhase);
            movePhase = false;
            switch (taskCreationPhase)
            {
                //タスクイメージ作成段階
                case TaskCreationPhase.Picture:
                    //タスク名（撮影もしくは選択時刻）を初期化
                    add_task_name = "";
                    //タスクの画像を撮影
                    if(picture_mode == true)
                    {
                        //撮影開始
                        StartToTakePicture();
                        yield return StartCoroutine(GetCameraTexture(data => add_task_image = data));
                        display_take.texture = add_task_image;
                    }
                    else
                    {
                        //選択開始
                        StartToChoicePicture();
                        yield return StartCoroutine(GetCameraRollTexture(data => add_task_image = data));
                        display_choice.texture = add_task_image;
                        // Debug.Log(add_task_image);
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
                    yield return StartCoroutine(CreateAndSave(data => taskData = data));
                    callBack(taskData);
                    button_parent.SetActive(true);
                    open_add_menu_button.SetActive(true);
                    hammer_button.SetActive(true);
                    home_button.SetActive(true);
                    history_button.SetActive(true);
                    config_button.SetActive(true);
                    yield break;
                default:
                    break;
            }
            while (movePhase == false)
                yield return null;
            if (canceled_to_add_task)
            {
                button_parent.SetActive(true);
                open_add_menu_button.SetActive(true);
                hammer_button.SetActive(true);
                home_button.SetActive(true);
                history_button.SetActive(true);
                config_button.SetActive(true);
                canceled_to_add_task = false;
                callBack(null);
                yield break;
            }
        }
    }

    //カメラで撮影したデータをテクスチャにして返す
    private IEnumerator GetCameraTexture(UnityAction<Texture2D> callBack)
    {
        //写真が取られるまで待機
        while (taked_picture == false)
        {
            yield return null;
            //フェーズが変わったら終わり
            if(taskCreationPhase != TaskCreationPhase.Picture)
            {
                // Debug.Log("撮影キャンセル");
                callBack(null);
                yield break;
            }
        }
        //撮影が完了したらウェブカメラをいったん停止する
        PauseToTakePicture();
        // Debug.Log("カメラ一時停止" + webCamTexture.isPlaying);
        //撮影データを取り込む
        Texture2D texture2D = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.ARGB32, false);
        texture2D.SetPixels(webCamTexture.GetPixels());
        texture2D.Apply();
        //タスク名を入力（現在は日付）
        add_task_name = System.DateTime.Now.ToLongDateString() + "_" + System.DateTime.Now.ToLongTimeString();
        //処理が終わるまで待つ
        yield return new WaitForEndOfFrame();
        //カメラを止める
        StopToTakePicture();
        //撮影データを返す（コルーチンのコールバック）
        callBack(texture2D);
        yield break;
    }


    //カメラロールから写真データを取得して返す
    private IEnumerator GetCameraRollTexture(UnityAction<Texture2D> callBack)
    {
        Texture2D texture2D = null;
        if(add_task_image != null){
            texture2D = add_task_image;
            // Debug.Log("タスクイメージのテクスチャを読み込み");
        }
        //画像が選択されるまで待機
        while(choiced_picture == false)
        {
            yield return null;
            if(taskCreationPhase != TaskCreationPhase.Picture)
            {
                if(add_task_image != null)
                    callBack(texture2D);
                yield break;
            }
        }
        texture2D = (Texture2D)display_choice.texture;
        //タスク名を入力（現在は日付）
        add_task_name = System.DateTime.Now.ToLongDateString() + "_" + System.DateTime.Now.ToLongTimeString();
        //処理が終わるまで待つ
        yield return new WaitForEndOfFrame();
        //取り込んだデータを返す
        callBack(texture2D);
        yield break;
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
        important_level_image_rect.localScale = change_scales[level];
        display_to_change_task_level.texture = add_task_image;
        important_level_text.text = "重要度:" + (level + 1);
        while (decided_important_level == false)
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
                    if(important_level_distance[0] <= dist && dist <= important_level_distance[0] + important_level_distance[1])
                    {
                        level = 0;
                    }
                    //重要度２
                    else if (important_level_distance[1] <= dist && dist <= important_level_distance[2])
                    {
                        level = 1;
                    }
                    //重要度３
                    else if(important_level_distance[2]<= dist)
                    {
                        level = 2;
                    }
                    important_level_text.text = "重要度:" + (level + 1);
                    important_level_image_rect.localScale = change_scales[level];
                }
            }
            //重要度が決まるまで待つs
            if (cancel_decided_important_level == true)
            {
                yield break;
            }

            yield return null;
        }
        yield return null;
        callBack(level);
        yield break;
    }

    //カレンダーに投げて、タスクの期限を設定する
    private IEnumerator SetLimitOfTask(UnityAction<string> callBack)
    {
        //カメラの描画対象のレイヤーを、期限設定用に切り替える
        Camera.main.cullingMask = task_limit_mask;
        //期限設定が完了するまで待機
        while (decide_task_limit == false)
        {
            yield return null;
            //期限設定がキャンセルされたら、カメラの描画対象を戻して戻る
            if(canceled_to_set_task_limit == true)
            {
                callBack(null);
                Camera.main.cullingMask = normal_mask;
                yield break;
            }
        }

        //期限設定が完了したらデータを入れて画面を戻す
        if(add_task_limit != null && add_task_limit != "")
        {
            Camera.main.cullingMask = normal_mask;
            set_task_limit_panel.SetActive(false);
            callBack(add_task_limit);
            yield break;
        }
    }

    //タスク作成とデータ保存と履歴への登録を行うメソッド
    private IEnumerator CreateAndSave(UnityAction<TaskData> callBack)
    {
        TaskData taskData = new TaskData();
        //タスクの画像を保存
        taskData.texture2D = add_task_image;
#if UNITY_ANDROID && !UNITY_EDITOR
        File.WriteAllBytes(Application.persistentDataPath + "/Images/" + add_task_name + ".png", add_task_image.EncodeToPNG());
#endif
        yield return new WaitForEndOfFrame();
        //タスクの名前を入れる
        taskData.task_name = add_task_name;
        //タスクの重要度を入れる
        taskData.task_important_level = (byte)add_task_important_level;
        //タスクの期限を入れる
        taskData.task_limit = add_task_limit;
        //タスク選択のテクスチャを初期化する
        display_choice.texture = null;

        //ここでデータをJsonファイルに書き込む（もしくは、生成後のタイミングでJsonファイルに書き込む）

        callBack(taskData);
        yield break;
    }


    //--------これ以降はフェーズ遷移と画面の切り替え関係のメソッド

    //撮影画面を表示する
    public void OpenTakePicturePanel()
    {
        take_picture_panel.SetActive(true);
        GoToNextPhase();
    }

    

    //撮影開始
    public void StartToTakePicture()
    {
        //ウェブカメラを作動させる
        webCamTexture.Play();
        //ウェブカメラに映っているものをRawImageに表示させる
        display_take.texture = webCamTexture;
        //写真を撮ったかどうかのフラグをオフにする
        taked_picture = false;
    }

    //取り直し
    //フェーズのenumの変数の値を変えずにフェーズの移動を許可することで、もう一度写真撮影もしくは選択段階に戻れる
    public void RestartToTakePicture()
    {
        movePhase = true;
    }

    //撮影
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
        webCamTexture.Stop();
    }

    //写真選択を開始する
    public void StartToChoicePicture(){
        choiced_picture = false;
    }

    //写真選択画面を表示する
    public void OpenChoicePicturePanel()
    {
        choice_picture_panel.SetActive(true);
        GoToNextPhase();
    }

    //写真選択を直す
    public void RechoicePicture(){
        movePhase = true;
        choiced_picture = false;
    }

    //写真選択を完了する
    public void DecideToChoicePicture()
    {
        if(display_choice.texture == null){
            return;
        }
        choiced_picture = true;
    }


    //重要度設定画面表示
    public void OpenTaskImportantLevelPanel()
    {
        if(picture_mode == true)
        {
            take_picture_panel.SetActive(false);
        }
        else
        {
            choice_picture_panel.SetActive(false);
        }
        GoToNextPhase();
        important_level_panel.SetActive(true);
        cancel_decided_important_level = false;
    }


    //重要度決定
    public void DecideImportantLevel()
    {
        decided_important_level = true;
        important_level_panel.SetActive(false);
        GoToNextPhase();
    }

    //重要度決定キャンセル
    public void CancelDecideImportantLevel()
    {
        cancel_decided_important_level = true;
        if (picture_mode == true)
        {
            take_picture_panel.SetActive(true);
            choice_picture_panel.SetActive(false);
            StartToTakePicture();
        }
        else
        {
            take_picture_panel.SetActive(false);
            choice_picture_panel.SetActive(true);
            choiced_picture = false;
        }
        BackToBeforePhase();
        important_level_panel.SetActive(false);
    }

    //期限設定
    public void OpenLimitPanel()
    {
        calender_maker.OpenCalender(System.DateTime.Now,add_task_image);
        set_task_limit_panel.SetActive(true);
        canceled_to_set_task_limit = false;
    }

    //タスクの期限決定
    public void DecideTaskLimit()
    {
        if(add_task_limit == null || add_task_limit == "")
            return;
        decide_task_limit = true;
        calender_maker.task_image = null;
        GoToNextPhase();
    }

    //タスク期限設定キャンセル
    public void CancelToSetLimit()
    {
        calender_maker.CloseCalener();
        //期限設定操作キャンセルフラグを立てる
        canceled_to_set_task_limit = true;
        //フェーズを下げる
        BackToBeforePhase();
        //期限設定パネルをオフ
        set_task_limit_panel.SetActive(false);
        //重要度設定パネルをオン
        important_level_panel.SetActive(true);
        cancel_decided_important_level = false;
        decided_important_level = false;
    }

    //通知に登録する
    private void AddTaskToNotice()
    {

    }

    //タスク追加操作をキャンセルする
    public void CancelToAddTask()
    {
        //RawImageのテクスチャを消す
        Destroy(add_task_image);
        //期限と重要度を消す
        add_task_important_level = 0;
        add_task_limit = null;
        webCamTexture.Stop();
        canceled_to_add_task = true;
        BackToBeforePhase();
        if (picture_mode == true)
            take_picture_panel.SetActive(false);
        else
            choice_picture_panel.SetActive(false);
    }
}
