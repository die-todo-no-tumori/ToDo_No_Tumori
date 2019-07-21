using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskInputFieldScript : MonoBehaviour
{
    [SerializeField]
    Task taskScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CloseTaskInput()
    {
        taskScript.InputCtoTagC();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
