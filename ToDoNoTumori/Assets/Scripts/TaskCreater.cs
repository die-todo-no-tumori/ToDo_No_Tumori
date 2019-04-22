using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskCreater : MonoBehaviour
{

    [SerializeField] private Slider prioritySlider;
    [SerializeField] private GameObject taskObject;
    [SerializeField] private Button taskCreateButton;

    void Start()
    {
        if (prioritySlider != null)
            prioritySlider.onValueChanged.AddListener(delegate { ChangeSliderColorByPriority(); });
        else Debug.Log("優先度スライダーはnullです");
        if (taskCreateButton != null)
            taskCreateButton.onClick.AddListener(CreateTask);
        else Debug.Log("タスク生成ボタンはnullです");
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

    private void CreateTask()
    {
        GameObject taskNameParent = transform.GetChild(0).gameObject;
        string taskName = taskNameParent.transform.GetChild(1).GetComponent<InputField>().text;
        taskNameParent.transform.GetChild(1).GetComponent<InputField>().text = "";
        GameObject priorityParent = transform.GetChild(1).gameObject;
        int priority = (int)priorityParent.transform.GetChild(1).GetComponent<Slider>().value;
        priorityParent.transform.GetChild(1).GetComponent<Slider>().value = 1;
        GameObject limitParent = transform.GetChild(2).gameObject;
        string limit = limitParent.transform.GetChild(1).GetComponent<InputField>().text +
            "/" + limitParent.transform.GetChild(3).GetComponent<InputField>().text +
            "/" + limitParent.transform.GetChild(5).GetComponent<InputField>().text +
            " " + limitParent.transform.GetChild(7).GetComponent<InputField>().text +
            ":" + limitParent.transform.GetChild(9).GetComponent<InputField>().text;

        limitParent.transform.GetChild(1).GetComponent<InputField>().text = "";
        limitParent.transform.GetChild(3).GetComponent<InputField>().text = "";
        limitParent.transform.GetChild(5).GetComponent<InputField>().text = "";
        limitParent.transform.GetChild(7).GetComponent<InputField>().text = "";
        limitParent.transform.GetChild(9).GetComponent<InputField>().text = "";

        GameObject taskSpawnPosObje = GameObject.Find("TaskSpawnPosObje");
        Task task = new Task(taskName, priority, limit);
        GameObject taskObje = Instantiate(taskObject, taskSpawnPosObje.transform.position, Quaternion.identity);
        taskObje.GetComponent<TaskObject>().task = task;
        taskObje.transform.SetParent(GameObject.Find("TaskParent").transform);
        gameObject.SetActive(false);
    }
}
