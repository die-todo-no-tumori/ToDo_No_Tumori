using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositiveButton : ButtonBase
{
    
    protected override void Start()
    {
        base.Start();
        push_sound = (AudioClip)Resources.Load("Audio/tdcr_decide");
        //GetComponent<Button>().onClick.AddListener(() =>
        //{
        //    TaskInputManager.GoToNextPhase();
        //    //OpenPanel();
        //});
    }

    void Update()
    {
        
    }

    //private void OpenPanel()
    //{
    //    if (positive_target_object != null)
    //        //return;
    //        positive_target_object.SetActive(true);
    //    if (negative_target_object != null)
    //        negative_target_object.SetActive(false);
    //}
}
