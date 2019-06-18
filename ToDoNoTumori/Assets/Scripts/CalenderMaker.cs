using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class CalenderMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject calender_cell;
    [SerializeField]
    private GameObject spawn_origin;
    [SerializeField]
    private RectTransform popup_pointer_rect;
    [SerializeField]
    private Camera target_camera;
    [SerializeField]
    private LayerMask layerMask;
    private string[] dayOfweek;
    private TaskInputManager taskInputManager;
    private string task_limit;

    void Start()
    {
        CreateCalender();
        //StartCoroutine(CreateCalender());
        taskInputManager = GameObject.Find("TaskInputManager").GetComponent<TaskInputManager>();
    }

    private void CreateCalender()
    {
        dayOfweek = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        CalenderCell[,] calender_cells = new CalenderCell[6, 7];

        for (int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 7; j++)
            {
                Vector3 pos = spawn_origin.transform.position;
                pos.x += j * 2;
                pos.y -= i * 2;
                GameObject cell = Instantiate(calender_cell, pos, Quaternion.identity);
                calender_cells[i, j] = cell.GetComponent<CalenderCell>();
                cell.transform.SetParent(spawn_origin.transform);
            }
        }

        DateTime first_date_time = new DateTime(DateTime.Now.Year,DateTime.Now.Month, 1);
        DateTime last_date_time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

        string first_date = first_date_time.DayOfWeek.ToString();
        int first_index = Array.IndexOf(dayOfweek, first_date);
        int day = 1;
        int date_index = first_index;
        int culum = 0;
        while(day <= last_date_time.Day)
        {
            calender_cells[culum, date_index].day = day;
            calender_cells[culum, date_index].gameObject.GetComponent<MeshRenderer>().material = (Material)Resources.Load("Materials/CC_" + day.ToString("00"));
            calender_cells[culum, date_index].gameObject.transform.Rotate(0, 180, 0);
            date_index++;
            if (date_index == 7)
            {
                date_index = 0;
                culum++;
            }
            day++;
        }

        foreach(CalenderCell calenderCell in calender_cells)
        {
            if(calenderCell.day == 0)
            {
                calenderCell.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        
        if (Input.touchCount == 1)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                Ray ray = target_camera.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if(Physics.Raycast(ray,out hit, 1000,layerMask))
                {
                    if(hit.collider.gameObject.tag == "CalenderCell")
                    {
                        Vector3 pos = popup_pointer_rect.transform.position;
                        Vector3 hitPos = target_camera.WorldToScreenPoint(hit.collider.transform.position);
                        pos.x = hitPos.x;
                        pos.y = hitPos.y;
                        popup_pointer_rect.transform.position = pos;
                        CalenderCell cell = hit.collider.GetComponent<CalenderCell>();
                        popup_pointer_rect.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "" + cell.day;
                        popup_pointer_rect.gameObject.SetActive(true);
                        task_limit = DateTime.Now.Month + ":" + cell.day;
                    }
                }
            }
        }
        else if(Input.touchCount == 0)
        {
            popup_pointer_rect.gameObject.SetActive(false);
        }
    }

    public void CloseScene()
    {
        taskInputManager.SetAddTaskLimit(task_limit);
        //taskInputManager.main_camera.gameObject.SetActive(true);

        StartCoroutine(CloseSceneCor());
    }

    public IEnumerator CloseSceneCor()
    {
        yield return SceneManager.UnloadSceneAsync("TaskList");
    }
}
