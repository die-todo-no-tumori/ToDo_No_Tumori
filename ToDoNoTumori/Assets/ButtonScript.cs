using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Branch branchScript;
    [SerializeField]
    private Message messageScript;
    [SerializeField]
    private Talk talkScript;
    private string message = "";
    public void OnClick()
    {
        Debug.Log("押された！");
        //messageScript.SetMessagePanel(message);
        //仮置きのID
        //string talkID = "branch_test";
        //ブランチ生成
        //branchScript.CreateBranch(talkID);

        //フリートークを開始する。（最初の会話が確定）
        talkScript.FirstTalk();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
