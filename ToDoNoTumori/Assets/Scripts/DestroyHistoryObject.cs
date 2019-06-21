using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyHistoryObject : HistoryObject
{
    [SerializeField]
    private Button respawn_button;

    void Start()
    {
        respawn_button.onClick.AddListener(() =>
        {

        });
    }



    void Update()
    {
        
    }
}
