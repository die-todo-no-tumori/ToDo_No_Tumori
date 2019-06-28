using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalenderCell : MonoBehaviour
{
    //[HideInInspector]
    public int day;
    [HideInInspector]
    public string date;

    public IEnumerator DestroyCor()
    {
        yield return null;
        Destroy(gameObject);
    }
}
