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
    }

    void Update()
    {
        
    }
}
