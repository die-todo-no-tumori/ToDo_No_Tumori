using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryPanelButton : ButtonBase
{
    [SerializeField]
    private GameObject[] historyPanels;

    protected override void Start()
    {
        base.Start();
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
    }

}
