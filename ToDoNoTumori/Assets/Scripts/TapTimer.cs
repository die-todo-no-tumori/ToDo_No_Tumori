using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapTimer
{

	private float total_tap_time;
	private float start_tap_time;
	private int count;
	void Start()
	{
		count = 0;
	}
	
	void Update()
	{
		if(Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Begin)
			{
				start_tap_time = Time.time;
			}
			else if(touch.phase == TouchPhase.End)
			{
				total_tap_time += Time.time - start_tap_time;
				count++;
			}
		}
	}




}