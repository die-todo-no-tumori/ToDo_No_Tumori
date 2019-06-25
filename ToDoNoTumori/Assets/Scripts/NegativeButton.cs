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
    }

    void Update()
    {
        
    }
}
