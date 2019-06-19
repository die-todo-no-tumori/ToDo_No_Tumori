using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTakePictureMenuButton : ButtonBase
{
    private bool isOpen;
    protected override void Start()
    {
        base.Start();
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

    void Update()
    {
        
    }
}
