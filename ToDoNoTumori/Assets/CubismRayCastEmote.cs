using UnityEngine;
using Live2D.Cubism.Framework.Raycasting;

public class CubismRayCastEmote : MonoBehaviour
{
    Animator animatorTasuco;
    private void Start()
    {
        animatorTasuco = GameObject.Find("tascomyu").GetComponent<Animator>();
    }
    private void Update()
    {
        // Return early in case of no user interaction.
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        /*
        if( animatorTasuco.GetBool("Enjoy") == true)
        {
            animatorTasuco.SetTrigger("Enjoy");
        }
        */

        if (Input.GetButtonDown("Fire1"))
        {
            // アニメーターで動かす
            animatorTasuco.SetBool("Enjoy", true);

            animatorTasuco.SetBool("Enjoy", false);
        }



        var raycaster = GetComponent<CubismRaycaster>();
        // Get up to 4 results of collision detection.
        var results = new CubismRaycastHit[4];


        // Cast ray from pointer position.
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hitCount = raycaster.Raycast(ray, results);


        // Show results.
        var resultsText = hitCount.ToString();
        for (var i = 0; i < hitCount; i++)
        {
            resultsText += "\n" + results[i].Drawable.name;
        }


        

        Debug.Log(resultsText);
    }
}