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


    void Start()
    {
        CreateCalender();
    }

    private void CreateCalender()
    {
        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 7; j++)
            {
                Vector3 pos = spawn_origin.transform.position;
                pos.x += j * 2;
                pos.y -= i * 2;
                Instantiate(calender_cell, pos, Quaternion.identity);
            }
        }
    }

    void Update()
    {
        
    }
}
