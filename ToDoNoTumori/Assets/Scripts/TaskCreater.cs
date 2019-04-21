using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskCreater : MonoBehaviour
{

    [SerializeField] private Slider prioritySlider;

    void Start()
    {
        prioritySlider.onValueChanged.AddListener(delegate { ChangeSliderColorByPriority(); });
    }

    void Update()
    {
        
    }

    //優先度のスライダーの値に応じてテキストの値を変える
    private void ChangeSliderColorByPriority()
    {
        //テキストの値を変える
        prioritySlider.transform.parent.GetChild(2).GetComponent<Text>().text = "" + prioritySlider.value;
    }
}
