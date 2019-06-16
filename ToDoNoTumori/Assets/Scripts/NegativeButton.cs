using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NegativeButton : ButtonBase
{
    protected override void Start()
    {
        base.Start();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            TaskInputManager.GoToNextPhase();
            ClosePanel();
        });
    }

    void Update()
    {
        
    }

    private void ClosePanel()
    {
        if (target_object == null)
            return;
        target_object.SetActive(false);
    }
}
