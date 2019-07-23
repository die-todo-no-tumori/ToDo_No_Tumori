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
        r
        XElement rootElement = xDocument.Element("root");

        var obj = rootElement.Elements("object");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
