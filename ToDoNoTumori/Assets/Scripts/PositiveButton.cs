using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositiveButton : ButtonBase
{
    
    protected override void Start()
    {
        base.Start();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            TaskInputManager.GoToNextPhase();
            OpenPanel();
        });
    }

    void Update()
    {
        
    }

    private void OpenPanel()
    {
        if (target_object == null)
            return;
        target_object.SetActive(true);
    }
}
