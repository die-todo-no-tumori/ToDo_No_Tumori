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

    void Start()
    {

    }

    //ボタンに貼り付ける
    public void MoveMonth(int value)
    {
        additional_month += value;
        List<CalenderCell> calenderCells = new List<CalenderCell>(spawn_origin.GetComponentsInChildren<CalenderCell>());
        for(int i = 0; i < calenderCells.Count;i++)
        {
            StartCoroutine(calenderCells[i].DestroyCor());
        }
        CreateCalender(DateTime.Now.AddMonths(additional_month));
    }

    public void OpenCalender(DateTime dateTime,Texture2D image)
    {
        task_image = image;
        spawn_origin.SetActive(true);
        CreateCalender(dateTime);
    }

    public void CloseCalener()
    {
        spawn_origin.SetActive(false);
    }

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
        
        if (Input.touchCount == 1)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit,100,layerMask))
                {
                    if(hit.collider.gameObject.tag == "CalenderCell")
                    {
                        Vector3 pos = popup_pointer_rect.transform.position;
                        Vector3 hitPos = Camera.main.WorldToScreenPoint(hit.collider.transform.position);
                        pos.x = hitPos.x;
                        pos.y = hitPos.y;
                        popup_pointer_rect.transform.position = pos;
                        CalenderCell cell = hit.collider.GetComponent<CalenderCell>();

                        if (cell.day != 0)
                        {
                            popup_pointer_rect.GetChild(0).GetChild(0).GetComponent<Text>().text = "" + cell.day;
                            popup_pointer_rect.gameObject.SetActive(true);
                            taskInputManager.add_task_limit = date_time.Month + ":" + cell.day;
                            if (ball == null)
                            {
                                ball = Instantiate(task_ball, cell.transform.GetChild(0).position, Quaternion.identity);
                            }
                            ball.transform.position = cell.transform.GetChild(0).transform.position;
                        }
                        else
                        {
                            popup_pointer_rect.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        else if(Input.touchCount == 0)
        {
            popup_pointer_rect.gameObject.SetActive(false);
        }
    }
}
