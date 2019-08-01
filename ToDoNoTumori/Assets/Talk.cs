using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    // Start is called before the first frame update
    //public string tempSourceID;
    //public string tempAttribute;
    public List<XMLDataMessage> tempTalkInfo = new List<XMLDataMessage>();
    [SerializeField]
    private XMLAnalyzer xmlScript;
    [SerializeField]
    private Branch branchScript;
    [SerializeField]
    private Message messageScript;
    void Start()
    {
        //List<XMLAnalyzer.XMLData> XMLReference = new List<XMLAnalyzer.XMLData>();
        XMLDataMessage data = new XMLDataMessage();
        data.id = null;
        //data.TargetID = null;
        data.attributes.Add(null);
        data.textMessage = null;

        tempTalkInfo.Add(data);
    }

    public void FirstTalk()
    {
        //List<XMLAnalyzer.XMLData> talkInfo;
        tempTalkInfo = xmlScript.SearchFirstMessage();
        //tempSourceID = talkInfo[0].SourceID;
        //tempAttribute = talkInfo[0].Attribute;
        messageScript.SetMessagePanel(tempTalkInfo[0].textMessage);

    }

    public void CreateTalk(List<XMLDataMessage> nextTalkInfo)
    {
        //string attribute;
        tempTalkInfo = nextTalkInfo;
        //attribute = tempTalkInfo[0].attributes[0];
        //Debug.Log(attribute);
        //tempAttribute = nextTalkInfo[0].Attribute;
        //tempSourceID = nextTalkInfo[0].SourceID;
        messageScript.SetMessagePanel(nextTalkInfo[0].textMessage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
