using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelToChangeILButton : ButtonBase
{
    [SerializeField]
    private TaskInputManager taskInputManager;
    [SerializeField]
    private GameObject take_picture_panel;
    [SerializeField]
    private GameObject choice_picture_panel;

    protected override void Start()
    {
        base.Start();
        push_sound = (AudioClip)Resources.Load("Audio/tdcr_cancel");
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            if(taskInputManager.picture_mode == true)
            {
                take_picture_panel.SetActive(true);
                choice_picture_panel.SetActive(false);
                taskInputManager.StartToTakePicture();
            }
            else
            {
                take_picture_panel.SetActive(false);
                choice_picture_panel.SetActive(true);
                taskInputManager.StartToTakePicture();
            }
            taskInputManager.CancelDecideImportantLevel();
            TaskInputManager.BackToBeforePhase();
            negative_target_object.SetActive(false);
        });
    }
}
