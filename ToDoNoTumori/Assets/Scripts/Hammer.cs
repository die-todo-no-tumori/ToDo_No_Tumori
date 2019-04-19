using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] private AudioSource sePlayer;
    [SerializeField] private AudioClip[] breakSounds;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject hit = GetRaycastHitObject();
            if (hit)
                Destroy(hit);
        }
    }

    //タスクオブジェクトの破壊と、破壊音の再生
    private GameObject GetRaycastHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if(Physics.Raycast(ray, out raycastHit , 10.0f))
        {
            GameObject obj = raycastHit.collider.gameObject;
            if (obj.tag == "Task")
            {
                return obj;
            }
        }
        return null;
    }
}
