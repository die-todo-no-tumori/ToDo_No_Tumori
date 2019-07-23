using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class BranchButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Branch branchScript;
    private string BranchText;
    
    
    
    void Start()
    {
        
    }

    public void OnClick()
    {
        BranchText = transform.GetComponentInChildren<Text>().text;
        

        if (BranchText == branchScript.NextMessageList[0].textMessage)
        {
            //一時的に次のメッセージの情報をリストとして保存。
            //List<XMLAnalyzer.XMLData> nextTalkInfo;

            //選択した選択肢がYesのとき
            //Yes選択肢に続くメッセージを検索、表示
            //xmlAnalyzerScript.SearchNextMessageを引数に会話を作成しなければならない
            //talkScript.tempTalkInfo = xmlAnalyzeScript.SearchNextMessage(branchScript.NextMessageList[0].SourceID);

            //talkScript.CreateTalk(xmlAnalyzeScript.SearchNextMessage(branchScript.NextMessageList[0].SourceID));

            //nextTalkInfo = xmlAnalyzeScript.SearchNextMessage(branchScript.NextMessageList[0].SourceID);
            //ShowListContentsInTheDebugLog<XMLAnalyzer.XMLData>(nextTalkInfo);

            //messageScript.SetMessagePanel(nextTalkInfo[0].SourceID);
            branchScript.CloseBranch("YES");

            //talkScript.CreateTalk(nextTalkInfo);
            
        }
        else if (BranchText == branchScript.NextMessageList[1].textMessage)
        {
            //一時的に次のメッセージの情報をリストとして保存
            //List<XMLAnalyzer.XMLData> nextTalkInfo;

            //選択した選択肢がYesのとき
            //Yes選択肢に続くメッセージの情報を検索、表示
            //talkScript.tempTalkInfo = xmlAnalyzeScript.SearchNextMessage(branchScript.NextMessageList[1].SourceID);

            //talkScript.CreateTalk(xmlAnalyzeScript.SearchNextMessage(branchScript.NextMessageList[0].SourceID));

            //nextTalkInfo = xmlAnalyzeScript.SearchNextMessage(branchScript.NextMessageList[1].TextMessage);
            //ShowListContentsInTheDebugLog<XMLAnalyzer.XMLData>(nextTalkInfo);

            //messageScript.SetMessagePanel(nextTalkInfo[0].SourceID);

            branchScript.CloseBranch("NO");
            
            //talkScript.CreateTalk(nextTalkInfo);
        }
    }

    public void ShowListContentsInTheDebugLog<T>(List<XMLDataMessage> list)
    {
        string log = "";

        foreach (var content in list.Select((val, idx) => new { val, idx }))
        {
            if (content.idx == list.Count - 1)
                log += content.val.ToString();
            else
                log += content.val.ToString() + ", ";
        }

        Debug.Log(log);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
