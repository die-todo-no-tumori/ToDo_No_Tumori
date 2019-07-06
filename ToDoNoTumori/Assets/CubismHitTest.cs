using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Framework.Raycasting;

public class CubismHitTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        var raycaster = GetComponent<CubismRaycaster>();

        var results = new CubismRaycastHit[4];

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hitCount = raycaster.Raycast(ray, results);

        var resultsText = hitCount.ToString();
        for(var i = 0; i < hitCount; i++)
        {
            resultsText += "\n" + results[i].Drawable.name;
        }

        Debug.Log(resultsText);
    }
}
