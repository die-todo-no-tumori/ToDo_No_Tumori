using System.IO;    
using System.Collections;
using UnityEngine;
using Unity.Notifications.Android;

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
    
    //[SerializeField]
    private string notification_channel_id;
    private string notification_channel_name;


    IEnumerator Start()
    {
    	notification_channel_id = "GrasPattChannel";
    	notification_channel_name = "GrasPatt";

#if UNITY_EDITOR
        DirectoryInfo directoryInfoImages = new DirectoryInfo(Application.dataPath + "/Images");
        if(directoryInfoImages.Exists == false){
            directoryInfoImages.Create();
            yield return null;
        }
        
        DirectoryInfo directoryInfoDatas = new DirectoryInfo(Application.dataPath + "/Datas");
        if(directoryInfoDatas.Exists == false){
            directoryInfoDatas.Create();
            yield return null;
        }

        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Datas/Data.json");
        if(fileInfo.Exists == false){
            fileInfo.Create();
            yield return null;
        }

        FileInfo fileInfoInput = new FileInfo(Application.dataPath + "/Datas/InputHistory.json");
        if(fileInfoInput.Exists == false){
            fileInfoInput.Create();
            yield return null;
        }
        
        FileInfo fileInfoDestroy = new FileInfo(Application.dataPath + "/Datas/DestroyHistory.json");
        if(fileInfoDestroy.Exists == false){
            fileInfoDestroy.Create();
            yield return null;
        }
        
        FileInfo fileInfoTotal = new FileInfo(Application.dataPath + "/Datas/TotalHistory.json");
        if(fileInfoTotal.Exists == false){
            fileInfoTotal.Create();
            yield return null;
        }
        yield return null;
        //タスクデータを読み込みとオブジェクトの生成
        LoadDatas();
#endif
        
#if UNITY_ANDROID && !UNITY_EDITOR

		//通知のチャンネルを作成して通知センターに登録
		CreateNotificationChannel();

		//保存する場所が存在するか確認
        //これは実機環境のみで作動
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
        yield return null;
        //タスクデータを読み込みとオブジェクトの生成
        LoadDatas();
#endif
    }

    void Update()
    {
        
    }

    //通知のチャンネル登録メソッド    
    private void CreateNotificationChannel()
    {
		AndroidNotificationChannel channel = new AndroidNotificationChannel
		{
			Id = notification_channel_id,
			Name = notification_channel_name,
			Importance = Importance.High,
			Description = "GrasPattの通知"
		};
		
		AndroidNotificationCenter.RegisterNotificationChannel(channel);
	}
	
    //通知情報を登録するメソッド
	public void AddNotification(TaskData taskData)
	{
		AndroidNotification notification = new AndroidNotification{
			Title = "期限の近いタスクがあります！",
			Text = "今すぐパッとGraspしましょう}",
			FireTime = System.DateTime.Now.AddSeconds(10)
		};
		
		AndroidNotificationCenter.SendNotification (notification , notification_channel_id);
	}

    //セーブ
    public void SaveDatas(){
        //タスクオブジェクトのデータを読み込む
        task_root = CollectTaskDatas();
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
        if(taskDataStr == null || taskDataStr == "")
            return false;
        task_root = ConvertToTaskData(taskDataStr);
        StartCoroutine(CreateTaskObjects(task_root));
        return true;
    }

    //履歴データを読み込み、履歴オブジェクトを生成する
    private bool LoadHistoryObjects(){
        //入力履歴
        string inputHistoryDataStr = ReadInputHistoryData();
        if(inputHistoryDataStr != null && inputHistoryDataStr != ""){
            input_history_root = ConvertToTaskData(inputHistoryDataStr);
            CreateInputHistoryObjects(input_history_root);
        }

        //破壊履歴
        string destroyHistoryDataStr = ReadDestroyHistoryData();
        if(destroyHistoryDataStr != null && destroyHistoryDataStr != ""){
            destory_history_root = ConvertToTaskData(destroyHistoryDataStr);
            CreateDestroyHistoryObjects(destory_history_root);
        }

        //総合履歴
        string totalHistoryDataStr = ReadTotalHistoryData();
        if(totalHistoryDataStr != null && totalHistoryDataStr != ""){
            total_history_root = ConvertToTaskData(totalHistoryDataStr);
            CreateTotalHistoryObjects(total_history_root);
        }
        return true;
    }

    
    //タスクデータの読み込み
    private string ReadTaskData()
    {
#if UNITY_EDITOR

        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Datas");
        if(directoryInfo.Exists == false)
            return null;

        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Datas/Data.json");
        if (fileInfo.Exists == false)
            return null;

        using(StreamReader streamReader = new StreamReader(Application.dataPath + "/Datas/Data.json"))
        {
            string data = streamReader.ReadToEnd();
            return data;
        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR

        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "/Datas");
        if(directoryInfo.Exists == false)
            return null;

        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Datas/Data.json");
        if (fileInfo.Exists == false)
            return null;

        using(StreamReader streamReader = new StreamReader(Application.persistentDataPath + "/Datas/Data.json"))
        {
            string data = streamReader.ReadToEnd();
            return data;
        }
#endif
    }

    //入力履歴データの読み込み
    private string ReadInputHistoryData()
    {
#if UNITY_EDITOR
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Datas");
        if (directoryInfo.Exists == false){
            return null;
        }

        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Datas/InputHistory.json");
        if (fileInfo.Exists == false)
            return null;

        using (StreamReader streamReader = new StreamReader(fileInfo.FullName))
        {
            string data = streamReader.ReadToEnd();
            return data;
        }
#endif


#if UNITY_ANDROID && !UNITY_EDITOR
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
#endif
    }

    //破壊履歴データの読み込み
    private string ReadDestroyHistoryData()
    {
#if UNITY_EDITOR
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Datas");
        if (directoryInfo.Exists == false){
            return null;
        }

        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Datas/DetroyHistory.json");
        if (fileInfo.Exists == false)
            return null;

        using (StreamReader streamReader = new StreamReader(fileInfo.FullName))
        {
            string data = streamReader.ReadToEnd();
            return data;
        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
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
#endif
    }

    //総合履歴データの読み込み
    private string ReadTotalHistoryData()
    {
#if UNITY_EDITOR
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Datas");
        if (directoryInfo.Exists == false){
            return null;
        }

        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Datas/TotalHistory.json");
        if (fileInfo.Exists == false)
            return null;

        using (StreamReader streamReader = new StreamReader(fileInfo.FullName))
        {
            string data = streamReader.ReadToEnd();
            return data;
        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
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
#endif
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
            byte[] textureData;
#if UNITY_ANDROID && !UNITY_EDITOR
            textureData = File.ReadAllBytes(Application.persistentDataPath + "/Images/" + taskData.task_name + ".png");
#elif UNITY_EDITOR
            textureData = File.ReadAllBytes(Application.dataPath + "/Images/" + taskData.task_name + ".png");
#endif
            Texture2D texture2D = new Texture2D(1200,1200,TextureFormat.ARGB32,false);
            texture2D.LoadImage(textureData);
            texture2D.Apply();
            taskData.texture2D = texture2D;
            applicationUser.InstantiateTaskObject(taskData,taskData.mode);
            yield return new WaitForSeconds(task_object_spawn_span);
        }
    }

    //入力履歴オブジェクトを生成する
    //生成メソッドはHistoryManagerクラスのものを利用
    private void CreateInputHistoryObjects(TaskRoot taskRoot){
        foreach(TaskData taskData in taskRoot.task_datas){
            byte[] textureData;
#if UNITY_ANDROID && !UNITY_EDITOR
            textureData = File.ReadAllBytes(Application.persistentDataPath + "/Images/" + taskData.task_name + ".png");
#elif UNITY_EDITOR
            textureData = File.ReadAllBytes(Application.dataPath + "/Images/" + taskData.task_name + ".png");
#endif
            Texture2D texture2D = new Texture2D(1200,1200,TextureFormat.ARGB32,false);
            texture2D.LoadImage(textureData);
            texture2D.Apply();
            taskData.texture2D = texture2D;
            historyManager.AddToInputHistory(taskData);
        }
    }

    //破壊履歴オブジェクトを生成する
    private void CreateDestroyHistoryObjects(TaskRoot taskRoot){
        foreach(TaskData taskData in taskRoot.task_datas){
            byte[] textureData;
#if UNITY_ANDROID && !UNITY_EDITOR
            textureData = File.ReadAllBytes(Application.persistentDataPath + "/Images/" + taskData.task_name + ".png");
#elif UNITY_EDITOR
            textureData = File.ReadAllBytes(Application.dataPath + "/Images/" + taskData.task_name + ".png");
#endif            
            Texture2D texture2D = new Texture2D(1200,1200,TextureFormat.ARGB32,false);
            texture2D.LoadImage(textureData);
            texture2D.Apply();
            taskData.texture2D = texture2D;
            historyManager.AddToDestroyHistory(taskData);
        }
    }

    //総合履歴オブジェクトを生成する
    private void CreateTotalHistoryObjects(TaskRoot taskRoot){
        foreach(TaskData taskData in taskRoot.task_datas){
            byte[] textureData;
#if UNITY_ANDROID && !UNITY_EDITOR
            textureData = File.ReadAllBytes(Application.persistentDataPath + "/Images/" + taskData.task_name + ".png");
#elif UNITY_EDITOR
            textureData = File.ReadAllBytes(Application.dataPath + "/Images/" + taskData.task_name + ".png");
#endif
            Texture2D texture2D = new Texture2D(1200,1200,TextureFormat.ARGB32,false);
            texture2D.LoadImage(textureData);
            texture2D.Apply();
            taskData.texture2D = texture2D;
            historyManager.AddToTotalHisttory(taskData);
        }
    }


    //タスクオブジェクトを探索してタスクデータを収集
    private TaskRoot CollectTaskDatas()
    {
        TaskRoot taskRoot = new TaskRoot();
        GameObject[] task_objects = GameObject.FindGameObjectsWithTag("TaskObject");
        foreach(GameObject task_object in task_objects)
        {
            taskRoot.task_datas.Add(task_object.GetComponent<TaskObject>().task_data);
        }
        return taskRoot;
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
#if UNITY_EDITOR
        //データの保存先ファイルが存在するかどうかを確認し、存在しなければ作成
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Datas/Data.json");
        if (fileInfo.Exists == false)
            fileInfo.Create();

        //画像を保存
        foreach(TaskData tData in taskRoot.task_datas){
            File.WriteAllBytes(Application.dataPath + "/Images/" + tData.task_name + ".png", tData.texture2D.EncodeToPNG());
        }

        string write_data = JsonUtility.ToJson(taskRoot);
        using(StreamWriter streamWriter = new StreamWriter(fileInfo.FullName))
        {
            streamWriter.Write(write_data);
        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR

        //データの保存先ファイルが存在するかどうかを確認し、存在しなければ作成
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Datas/Data.json");
        if (fileInfo.Exists == false)
            fileInfo.Create();

        //画像を保存
        foreach(TaskData tData in taskRoot.task_datas){
            File.WriteAllBytes(Application.persistentDataPath + "/Images/" + tData.task_name + ".png", tData.texture2D.EncodeToPNG());
        }

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
#if UNITY_EDITOR
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Datas/InputHistory.json");
        if (fileInfo.Exists == false)
            fileInfo.Create();

        string write_data = JsonUtility.ToJson(taskRoot);
        using(StreamWriter streamWriter = new StreamWriter(fileInfo.FullName))
        {
            streamWriter.Write(write_data);
        }
#endif

    
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
#if UNITY_EDITOR
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Datas/DestroyHistory.json");
        if (fileInfo.Exists == false)
            fileInfo.Create();

        string write_data = JsonUtility.ToJson(taskRoot);
        using(StreamWriter streamWriter = new StreamWriter(fileInfo.FullName))
        {
            streamWriter.Write(write_data);
        }
#endif



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
#if UNITY_EDITOR
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/Datas/TotalHistory.json");
        if (fileInfo.Exists == false)
            fileInfo.Create();

        string write_data = JsonUtility.ToJson(taskRoot);
        using(StreamWriter streamWriter = new StreamWriter(fileInfo.FullName))
        {
            streamWriter.Write(write_data);
        }
#endif



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
