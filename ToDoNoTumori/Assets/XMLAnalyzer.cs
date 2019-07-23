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

    public class XMLDataMessage{
        public string id;
        public string textMessage;
        public List<string> attributes;

        public XMLDataMessage(){
            attributes = new List<string>();
        }
    }

    public class XMLDataArrow{
        public string sourceID;
        public string targetID;
        public List<string> attributes;
        public XMLDataArrow(){
            attributes = new List<string>();
        }
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
        XMLData data = new XMLData();
        data.SourceID = "first001";
        data.TargetID = "branch001";
        data.Attributes.Add("Branch");
        data.TextMessage = FirstMessage;
        XMLReference.Add(data);

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
            XMLData data = new XMLData();
            data.SourceID = "branch001";
            data.TargetID = "talk002";
            data.Attributes.Add("Next");
            data.TextMessage = "はい";
            XMLReference.Add(data);

            XMLData data002 = new XMLData();
            data002.SourceID = "branch002";
            data002.TargetID = "talk003";
            data002.Attributes.Add("End");
            data002.TextMessage = "いいえ";
            XMLReference.Add(data002);


            //XMLReference.Add(new XMLData { SourceID = "branch001", TargetID = "talk002", Attribute = "Next", TextMessage = "はい" });
            //XMLReference.Add(new XMLData { SourceID = "branch002", TargetID = "talk003", Attribute = "End", TextMessage = "いいえ" });
        }
        else if(SourceID == "branch001")
        {
            XMLData data = new XMLData();
            data.SourceID = "talk002";
            data.TargetID = "null";
            data.Attributes.Add("AddTask");
            data.TextMessage = "ですよね！じゃあパパっと入力しちゃいましょう！";
            XMLReference.Add(data);

            //XMLReference.Add(new XMLData { SourceID = "talk002", TargetID = "null", Attribute = "AddTask", TextMessage = "ですよね！じゃあパパっと入力しちゃいましょう！" });
        }
        else if(SourceID == "branch002")
        {
            XMLData data = new XMLData();
            data.SourceID = "talk003";
            data.TargetID = "null";
            data.Attributes.Add("End");
            data.TextMessage = "そうですか…。わかりました！";
            XMLReference.Add(data);

            //XMLReference.Add(new XMLData { SourceID = "talk003", TargetID = "null", Attribute = "End", TextMessage = "そうですか…。わかりました！" });
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
