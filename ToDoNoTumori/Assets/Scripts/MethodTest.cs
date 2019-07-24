using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodTest : MonoBehaviour
{
    void Start()
    {
        NormalMethod();
    }

    void Update()
    {
        
    }

    private void NormalMethod(){
        MethodA();
        Debug.Log("insert");
        MethodB();
    }

    private IEnumerator CoroutineMethod(){
        yield return StartCoroutine(MethodACor());
        Debug.Log("insert cor");
        yield return StartCoroutine(MethodBCor());
    }


    private void MethodA(){
        Debug.Log("A");
    }

    private IEnumerator MethodACor(){
        Debug.Log("A Cor");
        yield return null;
    }

    private void MethodB(){
        Debug.Log("B");
    }

    private IEnumerator MethodBCor(){
        Debug.Log("B Cor");
        yield return null;
    }
}
