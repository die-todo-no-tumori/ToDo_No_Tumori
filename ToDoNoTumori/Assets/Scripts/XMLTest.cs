using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

public class XMLTest : MonoBehaviour
{
    [SerializeField]
    private XMLAnalyzer xMLAnalyzer;


    // Start is called before the first frame update
    void Start()
    {
        ReadXMLFile("commBasic1");
        Debug.Log(xMLAnalyzer.xmlDataMessage.Count);

    }


    private void ReadXMLFile(string fileName)
    {
        XDocument xDocument = XDocument.Load(Application.dataPath + @"\Resources\" + fileName + ".xml");

        XElement rootElement = xDocument.Element("mxGraphModel").Element("root");

        IEnumerable<XElement> xElements = rootElement.Elements("object");


        if(xMLAnalyzer.xmlDataArrow == null)
            xMLAnalyzer.xmlDataArrow = new List<XMLDataArrow>();
        if(xMLAnalyzer.xmlDataMessage == null)
            xMLAnalyzer.xmlDataMessage = new List<XMLDataMessage>();

        foreach (XElement element in xElements)
        {
            //矢印だったら
            if (element.Attribute("choice") != null)
            {
                XMLDataArrow arrow = new XMLDataArrow();
                arrow.choice = element.Attribute("choice").ToString();
                arrow.sourceID = element.Element("mxCell").Attribute("source").ToString();
                arrow.targetID = element.Element("mxCell").Attribute("target").ToString();
                xMLAnalyzer.xmlDataArrow.Add(arrow);
            }
            //それ以外だったら
            else
            {
                XMLDataMessage message = new XMLDataMessage();
                message.id = element.Attribute("id").ToString();
                message.textMessage = element.Attribute("text").ToString();
                xMLAnalyzer.xmlDataMessage.Add(message);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
