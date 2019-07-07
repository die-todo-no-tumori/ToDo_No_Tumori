using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Message messageScript;
    private string message = "はいはい！なんでしょうか！\n<>"
 + "ふふ～ん、なんだかヒマそうですねえ\n<>"
 + "そろそろ何か描きたくなってきたんじゃないですか？\n<>"
 + "今描きたいもの、教えてください！";
    public void OnClick()
    {
        Debug.Log("押された！");
        messageScript.SetMessagePanel(message);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
