using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskInputFieldScript : MonoBehaviour
{
    [SerializeField]
    Task taskScript;
    Text inputFieldText;

    // Start is called before the first frame update
    void Start()
    {
        inputFieldText = this.GetComponent<Text>();
    }

    public void CloseTaskInput()
    {
        taskScript.tempContext = inputFieldText.text;
        taskScript.InputCtoTagC();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
