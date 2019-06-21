using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTakePictureMenuButton : ButtonBase
{
    private bool isOpen;
    protected override void Start()
    {
        base.Start();
        push_sound = (AudioClip)Resources.Load("Audio/tdcr_decide");
        isOpen = false;
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            if (target_object == null)
                return;
            if(isOpen == false)
            {
                target_object.SetActive(true);
                isOpen = true;
            }
            else
            {
                target_object.SetActive(false);
                isOpen = false;
            }
        });
    }
}
