using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelToChangeILButton : ButtonBase
{
    // Start is called before the first frame update
    [SerializeField]
    private TaskInputManager taskInputManager;
    [SerializeField]
    private GameObject take_picture_panel;
    [SerializeField]
    private GameObject choice_picture_panel;
    protected override void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            if(taskInputManager.picture_mode == true)
            {
                take_picture_panel.SetActive(true);
            }
            else
            {
                take_picture_panel.SetActive(true);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
