using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] private GameObject taskInputPanel;
    private Button createTaskMenuButton;
    void Start()
    {
        createTaskMenuButton = transform.GetChild(0).GetComponent<Button>();
        createTaskMenuButton.onClick.AddListener(OpenCreateTaskMenu);
    }

    void Update()
    {
        
    }

    private void OpenCreateTaskMenu()
    {
        taskInputPanel.SetActive(true);
    }
}
