using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapTimer : MonoBehaviour
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
			if(touch.phase == TouchPhase.Began)
			{
				start_tap_time = Time.time;
			}
			else if(touch.phase == TouchPhase.Ended)
			{
				total_tap_time += Time.time - start_tap_time;
				Debug.Log(Time.time - start_tap_time);
				// count++;
				// if(count == 10)
				// 	Debug.Log(total_tap_time / count);
			}
		}
	}




}