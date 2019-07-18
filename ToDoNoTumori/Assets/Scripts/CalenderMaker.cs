using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CalenderMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject spawn_origin;
    [SerializeField]
    private RectTransform popup_pointer_rect;
    [SerializeField]
    private LayerMask layerMask;
    private string[] dayOfweek;
    [SerializeField]
    private TaskInputManager taskInputManager;
    [HideInInspector]
    public Texture2D task_image;
    [SerializeField]
    private GameObject task_ball;
    [HideInInspector]
    public GameObject ball;
    [SerializeField]
    private GameObject task_spawn_origin;
    [SerializeField]
    private GameObject[] calender_cell_objects;
    private DateTime date_time;
    [SerializeField]
    private Text month_text;
    private int additional_month;
    [SerializeField,Header("SEプレイヤー")]
    private AudioSource se_player_in_application_user;
    [SerializeField,Header("カレンダーオブジェクトを動かしたときの音")]
    private AudioClip caleder_sound;
    //カレンダーオブジェクトの以前の位置
    private Vector3 calender_object_pos_before;

    void Start()
    {
        calender_object_pos_before = Vector3.zero;
    }

    //月を移動するメソッド
    //ボタンに貼り付ける
    public void MoveMonth(int value)
    {
        additional_month += value;
        //まずはすでにあるカレンダーを破壊
        List<CalenderCell> calenderCells = new List<CalenderCell>(spawn_origin.GetComponentsInChildren<CalenderCell>());
        for(int i = 0; i < calenderCells.Count;i++)
        {
            StartCoroutine(calenderCells[i].DestroyCor());
        }
        //選択した月のカレンダーを作成する
        CreateCalender(DateTime.Now.AddMonths(additional_month));
    }

    //現在時刻と画像データを受け取り、カレンダーを開くs
    //期限設定の開始時に呼び出す
    public void OpenCalender(DateTime dateTime,Texture2D image)
    {
        task_image = image;
        if (ball != null)
            Destroy(ball.gameObject);
        spawn_origin.SetActive(true);
        CreateCalender(dateTime);
    }

    //カレンダーを閉じる
    public void CloseCalener()
    {
        List<CalenderCell> calenderCells = new List<CalenderCell>(spawn_origin.GetComponentsInChildren<CalenderCell>());
        for(int i = 0; i < calenderCells.Count;i++)
        {
            StartCoroutine(calenderCells[i].DestroyCor());
        }
        // spawn_origin.SetActive(false);
        Destroy(ball);
    }

    //カレンダーを作成するメソッド
    private void CreateCalender(DateTime dateTime)
    {
        month_text.text = "" + dateTime.Month + "月";
        date_time = dateTime;
        dayOfweek = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        DateTime first_date_time = new DateTime(dateTime.Year,dateTime.Month, 1);
        DateTime last_date_time = new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        string first_date = first_date_time.DayOfWeek.ToString();
        int first_index = Array.IndexOf(dayOfweek, first_date);
        int day = 1;
        int date_index = first_index;
        int culum = 0;

        //まずはカレンダーのデータを作成
        int[,] date_matrix = new int[6, 7];

        while(day <= last_date_time.Day)
        {
            date_matrix[culum, date_index] = day;
            date_index++;
            if (date_index == 7)
            {
                date_index = 0;
                culum++;
            }
            day++;
        }

        for (int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 7; j++)
            {
                Vector3 pos = spawn_origin.transform.position;
                pos.x += j * 2;
                pos.y -= i * 2;
                GameObject cell = Instantiate(calender_cell_objects[date_matrix[i,j]], pos,spawn_origin.transform.rotation);
                cell.GetComponent<CalenderCell>().day = date_matrix[i, j];
                cell.GetComponent<CalenderCell>().date = dayOfweek[j];
                if(date_matrix[i,j] != 0)
                {
                    cell.transform.Rotate(90, 0, 0);
                }
                cell.transform.SetParent(spawn_origin.transform);
            }
        }
    }

    void Update()
    {
        //振れている指が1本の時
        if (Input.touchCount == 1)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit,100,layerMask))
                {
                    //カレンダーセルに触れたら
                    if(hit.collider.gameObject.tag == "CalenderCell")
                    {
                        Vector3 pos = popup_pointer_rect.transform.position;
                        Vector3 hitPos = Camera.main.WorldToScreenPoint(hit.collider.transform.position);
                        pos.x = hitPos.x;
                        pos.y = hitPos.y;
                        //カレンダーのセルの位置に、日付の数値を表示するポップアップを出す
                        popup_pointer_rect.transform.position = pos;
                        //カレンダーセルのクラスを取得する
                        CalenderCell cell = hit.collider.GetComponent<CalenderCell>();

                        //日付が０でない、つまり情報が入っているカレンダーセルのとき
                        if (cell.day != 0)
                        {
                            //ポップアップの文字を日付の数値に変える
                            popup_pointer_rect.GetChild(0).GetChild(0).GetComponent<Text>().text = "" + cell.day;
                            popup_pointer_rect.gameObject.SetActive(true);
                            //タスクの期限を設定
                            taskInputManager.add_task_limit = date_time.Year + "/" +  date_time.Month + "/" + cell.day;
                            //最初にカレンダーに触れた時、カレンダーのボールを生成する
                            if (ball == null)
                            {
                                ball = Instantiate(task_ball, cell.transform.GetChild(0).position, Quaternion.identity);
                            }
                            //カレンダーボールの位置をカレンダーのセルの位置にする
                            ball.transform.position = cell.transform.GetChild(0).transform.position;
                            //カレンダーオブジェクトが移動したときのみ、移動音を再生
                            if(calender_object_pos_before == Vector3.zero){
                                calender_object_pos_before = cell.transform.GetChild(0).transform.position;
                            }else
                            {
                                if(calender_object_pos_before != cell.transform.GetChild(0).transform.position){
                                    calender_object_pos_before = cell.transform.GetChild(0).transform.position;
                                    se_player_in_application_user.PlayOneShot(caleder_sound);
                                }
                            }

                        }
                        else
                        {
                            //日付の数値が0の時は非表示にするs
                            popup_pointer_rect.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        //指を話したらポップアップを非表示にする
        else if(Input.touchCount == 0)
        {
            popup_pointer_rect.gameObject.SetActive(false);
        }
    }
}
