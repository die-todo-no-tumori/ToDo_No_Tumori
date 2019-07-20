using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMLAnalyzer : MonoBehaviour
{
    // Start is called before the first frame update
    private string TextMessage;
    private string Attribute;
    private string talkID;
    //private List<string> NextMessageList;

    public class XMLData
    {
        public string SourceID;
        public string TargetID;
        public string TextMessage;
        public string Attribute;
    }

    void Start()
    {
        
    }

    public List<XMLData> SearchFirstMessage()
    {
        string FirstMessage;
        //仮置き
        FirstMessage = "はいはい！なんでしょうか！\n<>"
 + "ふふ～ん、なんだかヒマそうですねえ\n<>"
 + "そろそろ何か描きたくなってきたんじゃないですか？";

        //シュードコード
        /*
            呼び出されると、属性がHead
        */
        // XMLのデータを保存
        List<XMLData> XMLReference = new List<XMLData>();
        XMLReference.Add(new XMLData { SourceID = "first001", TargetID = "branch001", Attribute = "Branch", TextMessage = FirstMessage });

        return XMLReference;
    }

    public List<XMLData> SearchNextMessage(string SourceID)
    {
        List<XMLData> XMLReference = new List<XMLData>();
        /*
        List<string> NextMessageList = new List<string>();//newで生成するのを忘れない
        NextMessageList.Add("TestMessage_001");
        NextMessageList.Add("TestMessage_002");
        */
        if (SourceID == "first001")
        {
            XMLReference.Add(new XMLData { SourceID = "branch001", TargetID = "talk002", Attribute = "Next", TextMessage = "はい" });
            XMLReference.Add(new XMLData { SourceID = "branch002", TargetID = "talk003", Attribute = "End", TextMessage = "いいえ" });
        }
        else if(SourceID == "branch001")
        {
            XMLReference.Add(new XMLData { SourceID = "talk002", TargetID = "null", Attribute = "AddTask", TextMessage = "ですよね！じゃあパパっと入力しちゃいましょう！" });
        }
        else if(SourceID == "branch002")
        {
            XMLReference.Add(new XMLData { SourceID = "talk003", TargetID = "null", Attribute = "End", TextMessage = "そうですか…。わかりました！" });
        }
        
        //仮置きのメッセージ

        //シュードコード
        /*
         talkIDを受け付ける;
         talkIDに対応したバルーンを見つける;
         そのバルーンに繋がっているバルーンの内容を全てリスト(NextMessageList)に入れる;
         */
        //return NextMessageList;
        return XMLReference;
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
