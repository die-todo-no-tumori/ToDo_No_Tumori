using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    // Start is called before the first frame update
    //public string tempSourceID;
    //public string tempAttribute;
    public List<XMLAnalyzer.XMLData> tempTalkInfo = new List<XMLAnalyzer.XMLData>
    {
        new XMLAnalyzer.XMLData{SourceID = "null", Attribute = "null", TargetID = "null", TextMessage = "null"}
    };
    [SerializeField]
    private XMLAnalyzer xmlScript;
    [SerializeField]
    private Branch branchScript;
    [SerializeField]
    private Message messageScript;
    void Start()
    {
        
    }

    public void FirstTalk()
    {
        //List<XMLAnalyzer.XMLData> talkInfo;
        tempTalkInfo = xmlScript.SearchFirstMessage();
        //tempSourceID = talkInfo[0].SourceID;
        //tempAttribute = talkInfo[0].Attribute;
        messageScript.SetMessagePanel(tempTalkInfo[0].TextMessage);

    }

    public void CreateTalk(List<XMLAnalyzer.XMLData> nextTalkInfo)
    {
        tempTalkInfo = nextTalkInfo;
        //tempAttribute = nextTalkInfo[0].Attribute;
        //tempSourceID = nextTalkInfo[0].SourceID;
        messageScript.SetMessagePanel(nextTalkInfo[0].TextMessage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
