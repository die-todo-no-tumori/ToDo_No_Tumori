using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCtoTagC : MonoBehaviour
{
    [SerializeField]
    Task taskScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TaskScriptLauncher()
    {
        taskScript.InputCtoTagC();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
