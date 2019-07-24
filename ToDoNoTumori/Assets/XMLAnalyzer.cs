using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMLDataMessage
{
    public string id;
    public string textMessage;
    public List<string> attributes;

    public XMLDataMessage()
    {
        attributes = new List<string>();
    }
}

public class XMLDataArrow
{
    public string choice;
    public string sourceID;
    public string targetID;
    public List<string> attributes;
    public XMLDataArrow()
    {
        attributes = new List<string>();
    }
}

public class XMLAnalyzer : MonoBehaviour
{
    // Start is called before the first frame update
    private string TextMessage;
    private string Attribute;
    private string talkID;
    //private List<string> NextMessageList;

    public List<XMLDataMessage> xmlDataMessage;
    public List<XMLDataArrow> xmlDataArrow;

    void Start()
    {
        
    }

    public List<XMLDataMessage> SearchFirstMessage()
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

        List<XMLDataMessage> XMLReference = new List<XMLDataMessage>();
        XMLDataMessage data = new XMLDataMessage();
        data.id = "first001";
        //data.TargetID = "branch001";
        data.attributes.Add("Branch");
        data.textMessage = FirstMessage;
        XMLReference.Add(data);

        return XMLReference;
    }

    public List<XMLDataMessage> SearchNextMessage(string SourceID)
    {
        List<XMLDataMessage> XMLReference = new List<XMLDataMessage>();
        
        /*
        List<string> NextMessageList = new List<string>();//newで生成するのを忘れない
        NextMessageList.Add("TestMessage_001");
        NextMessageList.Add("TestMessage_002");
        */

        //Arrowリストを初期化


        //引数のIDを持つArrowを探してArrowリストに追加


        //追加したArrowそれぞれのtargetIDに該当するMessageをメッセージリストに追加


        //メッセージリストを返す


        if (SourceID == "first001")
        {
            XMLDataMessage data = new XMLDataMessage();
            data.id = "branch001";
            //data.TargetID = "talk002";
            data.attributes.Add("Next");
            data.textMessage = "はい";
            XMLReference.Add(data);

            XMLDataMessage data002 = new XMLDataMessage();
            data002.id = "branch002";
            //data002.TargetID = "talk003";
            data002.attributes.Add("End");
            data002.textMessage = "いいえ";
            XMLReference.Add(data002);


            //XMLReference.Add(new XMLData { SourceID = "branch001", TargetID = "talk002", Attribute = "Next", TextMessage = "はい" });
            //XMLReference.Add(new XMLData { SourceID = "branch002", TargetID = "talk003", Attribute = "End", TextMessage = "いいえ" });
        }
        else if(SourceID == "branch001")
        {
            XMLDataMessage data = new XMLDataMessage();
            data.id = "talk002";
            //data.TargetID = "null";
            data.attributes.Add("AddTask");
            data.textMessage = "ですよね！じゃあパパっと入力しちゃいましょう！";
            XMLReference.Add(data);

            //XMLReference.Add(new XMLData { SourceID = "talk002", TargetID = "null", Attribute = "AddTask", TextMessage = "ですよね！じゃあパパっと入力しちゃいましょう！" });
        }
        else if(SourceID == "branch002")
        {
            XMLDataMessage data = new XMLDataMessage();
            data.id = "talk003";
            //data.TargetID = "null";
            data.attributes.Add("End");
            data.textMessage = "そうですか…。わかりました！";
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
