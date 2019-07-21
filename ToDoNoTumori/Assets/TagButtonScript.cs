using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagButtonScript : MonoBehaviour
{
    [SerializeField]
    Task taskScript;
    GameObject TagCanvas;
    GameObject TagPanel;
    // Start is called before the first frame update
    void Start()
    {
        TagCanvas = this.transform.root.gameObject;
        TagPanel = TagCanvas.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    public void onClick()
    {
        switch(this.name)
        {
            case "AnimeButton":
                taskScript.tempTag = "Anime";
                break;

            case "MovieButton":
                taskScript.tempTag = "Movie";
                break;

            case "OtherButton":
                taskScript.tempTag = "Other";
                break;

            case "NovelButton":
                taskScript.tempTag = "Novel";
                break;

            case "ArtButton":
                taskScript.tempTag = "Art";
                break;
        }

        taskScript.WriteTaskContentsinMemory();
        TagPanel.SetActive(false);
        
    }
}
