using UnityEngine;
using System.IO;    
using System.Collections;

public class SaveAndLoader : MonoBehaviour
{
    private TaskRoot task_root;
    private TaskRoot input_history_root;
    private TaskRoot destory_history_root;
    private TaskRoot total_history_root;

    [SerializeField]
    private GameObject input_history_parent;
    [SerializeField]
    private GameObject destroy_history_parent;
    [SerializeField]
    private GameObject total_history_parent;

    [SerializeField]
    private ApplicationUser applicationUser;
    [SerializeField]
    private HistoryManager historyManager;
    [SerializeField]
    private float task_object_spawn_span;


    IEnumerator Start()
    {
        //保存する場所が存在するか確認
#if UNITY_ANDROID && !UNITY_EDITOR
        DirectoryInfo directoryInfoImages = new DirectoryInfo(Application.persistentDataPath + "/Images");
        if(directoryInfoImages.Exists == false){
            directoryInfoImages.Create();
            yield return null;
        }
        
        DirectoryInfo directoryInfoDatas = new DirectoryInfo(Application.persistentDataPath + "/Datas");
        if(directoryInfoDatas.Exists == false){
            directoryInfoDatas.Create();
            yield return null;
        }

        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Datas/Data.json");
        if(fileInfo.Exists == false){
            fileInfo.Create();
            yield return null;
        }

        FileInfo fileInfoInput = new FileInfo(Application.persistentDataPath + "/Datas/InputHistory.json");
        if(fileInfoInput.Exists == false){
            fileInfoInput.Create();
            yield return null;
        }
        
        FileInfo fileInfoDestroy = new FileInfo(Application.persistentDataPath + "/Datas/DestroyHistory.json");
        if(fileInfoDestroy.Exists == false){
            fileInfoDestroy.Create();
            yield return null;
        }
        
        FileInfo fileInfoTotal = new FileInfo(Application.persistentDataPath + "/Datas/TotalHistory.json");
        if(fileInfoTotal.Exists == false){
            fileInfoTotal.Create();
            yield return null;
        }
#endif
        yield return null;
        //タスクデータを読み込みとオブジェクトの生成
        LoadDatas();

    }

    void Update()
    {
        
    }

    //セーブ
    public void SaveDatas(){
        //タスクオブジェクトのデータを読み込む
        task_root = CollectTaskDatas();

        //タスクオブジェクトのデータを書き込む
        WriteTaskData(task_root);

        //入力履歴データを読み込み
        input_history_root = CollectHistoryData(input_history_parent);
        WriteInputHistoryData(input_history_root);

        //破壊履歴データを読み込み
        destory_history_root = CollectHistoryData(destroy_history_parent);
        WriteDestroyHistoryData(destory_history_root);

        //総合履歴データを読み込み
        total_history_root = CollectHistoryData(total_history_parent);
        WriteTotalHistoryData(total_history_root);
    }


    //ローディング
    private void LoadDatas(){
        LoadTaskObjects();
        LoadHistoryObjects();
    }

    //タスクデータを読み込み、タスクオブジェクトを生成する
    private bool LoadTaskObjects(){
        string taskDataStr = ReadTaskData();
        if(taskDataStr == null)
            return false;
        task_root = ConvertToTaskData(taskDataStr);
        StartCoroutine(CreateTaskObjects(task_root));
        return true;
    }

    //履歴データを読み込み、履歴オブジェクトを生成する
    private bool LoadHistoryObjects(){
        //入力履歴
        string inputHistoryDataStr = ReadInputHistoryData();
        if(inputHistoryDataStr == null)
            return false;
        input_history_root = ConvertToTaskData(inputHistoryDataStr);
        CreateInputHistoryObjects(input_history_root);
        
        //出力履歴
        string destroyHistoryDataStr = ReadDestroyHistoryData();
        if(destroyHistoryDataStr == null)
            return false;
        destory_history_root = ConvertToTaskData(destroyHistoryDataStr);
        CreateDestroyHistoryObjects(destory_history_root);

        //総合履歴
        string TotalHistoryDataStr = ReadTotalHistoryData();
        if(TotalHistoryDataStr == null)
            return false;
        total_history_root = ConvertToTaskData(TotalHistoryDataStr);
        CreateTotalHistoryObjects(total_history_root);

        return true;
    }

    
    //タスクデータの読み込み
    private string ReadTaskData()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/Datas");
        if(directoryInfo.Exists == false)
            return null;

        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Datas/Data.json");
        if (fileInfo.Exists == false)
            return null;

        using(StreamReader streamReader = new StreamReader(fileInfo.FullName))
        {
            string data = streamReader.ReadToEnd();
            return data;
        }
    }

    //入力履歴データの読み込み
    private string ReadInputHistoryData()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/Datas");
        if (directoryInfo.Exists == false)
            return null;

        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Datas/InputHistory.json");
        if (fileInfo.Exists == false)
            return null;

        using (StreamReader streamReader = new StreamReader(fileInfo.FullName))
        {
            string data = streamReader.ReadToEnd();
            return data;
        }
    }

    //破壊履歴データの読み込み
    private string ReadDestroyHistoryData()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/Datas");
        if (directoryInfo.Exists == false)
            return null;

        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Datas/DetroyHistory.json");
        if (fileInfo.Exists == false)
            return null;

        using (StreamReader streamReader = new StreamReader(fileInfo.FullName))
        {
            string data = streamReader.ReadToEnd();
            return data;
        }
    }

    //総合履歴データの読み込み
    private string ReadTotalHistoryData()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/Datas");
        if (directoryInfo.Exists == false)
            return null;

        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Datas/TotalHistory.json");
        if (fileInfo.Exists == false)
            return null;

        using (StreamReader streamReader = new StreamReader(fileInfo.FullName))
        {
            string data = streamReader.ReadToEnd();
            return data;
        }
    }


    //読み込んだタスクデータをタスククラスに変換
    private TaskRoot ConvertToTaskData(string data)
    {
        TaskRoot taskRoot = new TaskRoot();
        taskRoot = JsonUtility.FromJson<TaskRoot>(data);
        return taskRoot;
    }

    //タスクデータを持つタスクオブジェクトを生成する
    //生成メソッドはApplicationUserクラスのものを利用
    private IEnumerator CreateTaskObjects(TaskRoot taskRoot)
    {
        foreach(TaskData taskData in taskRoot.task_datas){
            applicationUser.InstantiateTaskObject(taskData);
            yield return new WaitForSeconds(task_object_spawn_span);
        }
    }

    //入力履歴オブジェクトを生成する
    //生成メソッドはHistoryManagerクラスのものを利用
    private void CreateInputHistoryObjects(TaskRoot taskRoot){
        foreach(TaskData taskData in taskRoot.task_datas){
            historyManager.AddToInputHistory(taskData);
        }
    }

    //破壊履歴オブジェクトを生成する
    private void CreateDestroyHistoryObjects(TaskRoot taskRoot){
        foreach(TaskData taskData in taskRoot.task_datas){
            historyManager.AddToInputHistory(taskData);
        }
    }

    //総合履歴オブジェクトを生成する
    private void CreateTotalHistoryObjects(TaskRoot taskRoot){
        foreach(TaskData taskData in taskRoot.task_datas){
            historyManager.AddToInputHistory(taskData);
        }
    }


    //タスクオブジェクトを探索してタスクデータを収集
    private TaskRoot CollectTaskDatas()
    {
        TaskRoot task_root = new TaskRoot();
        GameObject[] task_objects = GameObject.FindGameObjectsWithTag("TaskObject");
        foreach(GameObject task_object in task_objects)
        {
            task_root.task_datas.Add(task_object.GetComponent<TaskObject>().task_data);
        }
        return task_root;
    }

    //履歴オブジェクトを探索して、データを収集
    private TaskRoot CollectHistoryData(GameObject parent){
        TaskRoot taskRoot = new TaskRoot();
        HistoryObject[] historyObjects = parent.GetComponentsInChildren<HistoryObject>();
        foreach(HistoryObject historyObject in historyObjects){
            taskRoot.task_datas.Add(historyObject.task_data);
        }
        return taskRoot;
    }

    //タスクデータを書き込み
    private void WriteTaskData(TaskRoot taskRoot)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Datas/Data.json");
        if (fileInfo.Exists == false)
            fileInfo.Create();

        string write_data = JsonUtility.ToJson(taskRoot);
        using(StreamWriter streamWriter = new StreamWriter(fileInfo.FullName))
        {
            streamWriter.Write(write_data);
        }
        #endif
    }

    //入力履歴データを書き込み
    private void WriteInputHistoryData(TaskRoot taskRoot)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Datas/InputHistory.json");
        if (fileInfo.Exists == false)
            fileInfo.Create();

        string write_data = JsonUtility.ToJson(taskRoot);
        using(StreamWriter streamWriter = new StreamWriter(fileInfo.FullName))
        {
            streamWriter.Write(write_data);
        }
        #endif
    }

    //破壊履歴データを書き込み
    private void WriteDestroyHistoryData(TaskRoot taskRoot)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Datas/DestroyHistory.json");
        if (fileInfo.Exists == false)
            fileInfo.Create();

        string write_data = JsonUtility.ToJson(taskRoot);
        using(StreamWriter streamWriter = new StreamWriter(fileInfo.FullName))
        {
            streamWriter.Write(write_data);
        }
        #endif
    }

    //総合履歴データを書き込み
    private void WriteTotalHistoryData(TaskRoot taskRoot)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Datas/TotalHistory.json");
        if (fileInfo.Exists == false)
            fileInfo.Create();

        string write_data = JsonUtility.ToJson(taskRoot);
        using(StreamWriter streamWriter = new StreamWriter(fileInfo.FullName))
        {
            streamWriter.Write(write_data);
        }
        #endif
    }
}
