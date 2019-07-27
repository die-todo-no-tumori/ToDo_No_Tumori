using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Branch : MonoBehaviour
{
    [SerializeField]
    public Text YesText;
    [SerializeField]
    public Text NoText;
    [SerializeField]
    private XMLAnalyzer xMLAnalyzer;
    [SerializeField]
    private Talk talkScript;
    //次のメッセージを格納した配列
    public List<XMLDataMessage> NextMessageList;

    //private string talkID;
    // Start is called before the first frame update
    void Start()
    {
        //イエスとノーの選択肢のテキストコンポーネントを取得
        /*
        YesText = transform.Find("Panel/YesBrunchButton/Text").GetComponent<Text>();
        NoText = transform.Find("Panel/NoBrunchButton/Text").GetComponent<Text>();
        */
        //XMLAnalyzerを取得
        //xMLAnalyzer = GameObject.Find("XMLAnalyzer").GetComponent<XMLAnalyzer>();

        transform.GetChild(0).gameObject.SetActive(false);

    }

    public void CreateBranch(string sourceID)
    {
        //選択肢画面を表示
        transform.GetChild(0).gameObject.SetActive(true);

        NextMessageList = xMLAnalyzer.SearchNextMessage(sourceID);
        //メッセージ内容をイエスとノーの選択肢のテキストに流し込み
        YesText.text = NextMessageList[0].textMessage;
        NoText.text = NextMessageList[1].textMessage;

        /*
        for (int ArrayCount = 0; ArrayCount <=  xMLAnalyzer.SearchNextMessage(talkID).Count ; ArrayCount++)
        {
            
        }


        */
        //if (sourceID == "first001")
        //{
            
        //}
        
    }

    public void CloseBranch(string YESorNO)
    {
        //選択肢に続くメッセージを表示する
        if(YESorNO == "YES")
        {
            talkScript.CreateTalk(xMLAnalyzer.SearchNextMessage(NextMessageList[0].id));
        }
        else if(YESorNO == "NO")
        {
            talkScript.CreateTalk(xMLAnalyzer.SearchNextMessage(NextMessageList[1].id));
        }
        
        //選択肢画面を閉じる
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
