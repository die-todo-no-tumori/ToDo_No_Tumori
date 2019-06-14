using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoroutineTest : MonoBehaviour
{


    void Start()
    {
        StartCoroutine(Cor1());
        Debug.Log(4);
    }




    void Update()
    {
        
    }

    IEnumerator Cor1()
    {
        Debug.Log(1);
        Test1();
        Debug.Log(3);
        yield break;
    }

    IEnumerator Cor2(int val)
    {
        Debug.Log(val);
        yield break;
    }

    void Test1()
    {
        StartCoroutine(Cor2(2));
    }


}
