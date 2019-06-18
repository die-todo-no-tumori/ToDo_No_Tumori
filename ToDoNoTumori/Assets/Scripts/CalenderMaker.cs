using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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


    void Start()
    {
        CreateCalender();
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

        DateTime first_date_time = new DateTime(DateTime.Now.Year,DateTime.Now.AddMonths(1).Month, 1);
        DateTime last_date_time = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month));

        //Debug.Log(date.DayOfWeek.ToString());

        string first_date = first_date_time.DayOfWeek.ToString();
        int first_index = Array.IndexOf(dayOfweek, first_date);
        //Debug.Log(first_index);
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
                //Debug.Log(Input.GetTouch(0).position.x + " , " + Input.GetTouch(0).position.y);
                //Debug.Log(ray.origin.x + " , " + ray.origin.y);
                //Debug.DrawRay(Input.GetTouch(0).position, Camera.main.ScreenPointToRay(Input.GetTouch(0).position).direction,Color.red);
                RaycastHit hit;
                if(Physics.Raycast(ray,out hit, 1000,layerMask))
                {
                    if(hit.collider.gameObject.tag == "CalenderCell")
                    {
                        //Debug.Log(hit.collider.gameObject.name);

                        Vector3 pos = popup_pointer_rect.transform.position;
                        Vector3 hitPos = target_camera.WorldToScreenPoint(hit.collider.transform.position);
                        pos.x = hitPos.x;
                        pos.y = hitPos.y;
                        popup_pointer_rect.transform.position = pos;
                        popup_pointer_rect.gameObject.SetActive(true);
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
