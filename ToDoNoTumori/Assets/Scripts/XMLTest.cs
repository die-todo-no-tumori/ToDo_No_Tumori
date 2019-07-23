using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

public class XMLTest : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        XDocument xDocument = new XDocument(Application.dataPath + "/Resources/commBasic1.xml");
        
        XElement rootElement = xDocument.Element("mxGraphModel").Element("root");

        IEnumerable<XElement> xElements = rootElement.Elements("object");

        foreach(XElement element in xElements)
        {
            //矢印だったら
            if(element.Attribute("choice") != null)
            {

            }
            //それ以外だったら
            else
            {

            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
