using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

[System.Serializable]
public class TaskData
{
    //名前
    //public string name;
    //内容
    public string context;
    //タグ
    public string tag;
    //期限
    public string limit;
}

[System.Serializable]
public class TaskDataRoot
{
    public List<TaskData> taskRoot;
    public TaskDataRoot()
    {
        taskRoot = new List<TaskData>();
    }
}


public class Task : MonoBehaviour
{
    // Start is called before the first frame update
    public TaskDataRoot taskDataRoot;
    TouchScreenKeyboard keyboard;

    //Inputオブジェクトを取得
    [SerializeField]
    GameObject InputCanvasPanel;
    [SerializeField]
    Text InputCanvasText;
    [SerializeField]
    GameObject TagCanvasPanel;
    [SerializeField]
    InputField inputField;

    public string tempContext = "null";
    public string tempTag = "null";
    public string tempLimit = "null";

    void Start()
    {
        //InputCanvasText = InputCanvas.transform.Find("Panel/InputField/Back/InputField/Text").GetComponent<Text>();

        inputField = GetComponent<InputField>();
        InputCanvasPanel.SetActive(false);
        TagCanvasPanel.SetActive(false);
    }

    public void OpenInputCanvas()
    {
        //タスクの内容を入力するインプットフィールドを表示した後にキーボードを表示するメソッド

        //InputCanvasのテキスト・タグ入力UIの表示
        InputCanvasPanel.SetActive(true);

        //タスクの内容作成
        //キーボード入力からタスクの内容を入力
        //キーボードの表示
        //this.keyboard = TouchScreenKeyboard.Open("タスクを入力してね", TouchScreenKeyboardType.Default);
    }

    public void InputCtoTagC()
    {
        //this.tempContext = this.keyboard.text;
        tempContext = inputField.text;
        InputCanvasPanel.SetActive(false);
        TagCanvasPanel.SetActive(true);
    }


    public void SelectTag()
    {
        //タグ選択画面を表示して選ばせた後にtempTagへそのタグ名を格納するメソッド
        

        switch (TagCanvasPanel.transform.Find("Panel/TagFieldBack").GetChild(0).name)
        {
            case "AnimeButton":
                tempTag = "Anime";
                break;

            case "MovieButton":
                tempTag = "Movie";
                break;

            case "OtherButton":
                tempTag = "Other";
                break;

            case "NovelButton":
                tempTag = "Novel";
                break;

            case "ArtButton":
                tempTag = "Art";
                break;
        }

        TagCanvasPanel.SetActive(false);
        WriteTaskContentsinMemory();
    }

    public void WriteTaskContentsinMemory()
    {

        //タスクのルートを作成
        TaskDataRoot root = new TaskDataRoot();

        //temp変数をrootに代入
        TaskData data = new TaskData();
        data.context = tempContext;
        data.tag = tempTag;
        data.tag = tempLimit;
        root.taskRoot.Add(data);

        //jsonデータ作成
        string jsonData = JsonUtility.ToJson(root);
        StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/" + "Data" + ".json");
        streamWriter.Write(jsonData);
        streamWriter.Close();
    }



    // Update is called once per frame
    void Update()
    {
        /*
        if (InputCanvasPanel.activeSelf == true)
        {
            InputCanvasText.text = this.keyboard.text;
        }
        */
    }
}
