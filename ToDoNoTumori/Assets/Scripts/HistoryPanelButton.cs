using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryPanelButton : ButtonBase
{
    [SerializeField]
    private GameObject[] historyPanels;
    [SerializeField]
    private Button input_button;
    [SerializeField]
    private Button destroy_button;
    [SerializeField]
    private Button total_button;

    protected override void Start()
    {
        base.Start();
        push_sound = (AudioClip)Resources.Load("Audio/tdcr_decide");
    }

    public void ShowPanel(int panelIndex)
    {
        for(int i = 0; i < historyPanels.Length; i++)
        {
            if (i == panelIndex)
            {
                historyPanels[i].SetActive(true);

            }
            else
                historyPanels[i].SetActive(false);
        }

        GetComponent<Button>().enabled = false;

        switch (panelIndex)
        {
            case 0:
                destroy_button.enabled = true;
                total_button.enabled = true;
                break;
            case 1:
                input_button.enabled = true;
                total_button.enabled = true;
                break;
            case 2:
                input_button.enabled = true;
                destroy_button.enabled = true;
                break;
            default:
                break;
        }
    }

}
