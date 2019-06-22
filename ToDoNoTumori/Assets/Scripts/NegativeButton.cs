using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NegativeButton : ButtonBase
{
    protected override void Start()
    {
        base.Start();
        push_sound = (AudioClip)Resources.Load("Audio/tdcr_cancel");
        //GetComponent<Button>().onClick.AddListener(() =>
        //{
        //    TaskInputManager.GoToNextPhase();
        //    //ClosePanel();
        //});
    }

    void Update()
    {
        
    }

    //private void ClosePanel()
    //{
    //    if (positive_target_object != null)
    //        //return;
    //        positive_target_object.SetActive(true);
    //    if (negative_target_object != null)
    //        //return;
    //        negative_target_object.SetActive(false);

    //}
}
