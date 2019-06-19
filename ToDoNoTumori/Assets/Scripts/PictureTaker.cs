using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PictureTaker : MonoBehaviour
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private RawImage display;

    private WebCamTexture webCamTexture;


    private IEnumerator Start()
    {
        if (WebCamTexture.devices.Length == 0)
        {
            yield break;
        }

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            yield break;
        }

        WebCamDevice userCameraDevice = WebCamTexture.devices[0];
        webCamTexture = new WebCamTexture(userCameraDevice.name, width, height);

        display.texture = webCamTexture;
        webCamTexture.Stop();

    }

    //撮影開始
    public void StartToTakePicture()
    {
        if(webCamTexture.isPlaying == false)
            webCamTexture.Play();
    }

    //撮影一時停止
    public void PauseToTakePicture()
    {
        if (webCamTexture.isPlaying == false) return;
        webCamTexture.Pause();
        
    }

    //撮影停止
    public void StopToTakePicture()
    {
        if (webCamTexture.isPlaying)
            webCamTexture.Stop();
    }



    //public IEnumerator TakePicture()
    //{
    //    PauseToTakePicture();
    //    Texture2D texture2D = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.ARGB32, false);
    //    texture2D.SetPixels(webCamTexture.GetPixels());
    //    texture2D.Apply();
    //    yield return new WaitForEndOfFrame();

    //    //byte[] png = texture2D.EncodeToPNG();
    //    //Destroy(texture2D);
    //    //File.WriteAllBytes(Application.persistentDataPath + "/Images/Test.png", png);
    //    //yield return new WaitForEndOfFrame();
    //}


    //    private void OnGUI()
    //    {
    //        GUILayoutOption[] gUILayoutOptions = { GUILayout.Height(150f), GUILayout.Width(500f) };
    //        if (GUILayout.Button("Stop",gUILayoutOptions))
    //        {

    //            if (webCamTexture.isPlaying)
    //                webCamTexture.Stop();
    //            Texture2D texture = (Texture2D)Resources.Load("Test");
    //            display.texture = texture;
    //        }
    //        else if (GUILayout.Button("Pause",gUILayoutOptions))
    //        {
    //            if (webCamTexture.isPlaying)
    //            {
    //                webCamTexture.Pause();

    //                Texture2D texture2D = new Texture2D(webCamTexture.width,webCamTexture.height, TextureFormat.ARGB32, false);
    //                texture2D.SetPixels(webCamTexture.GetPixels());
    //                texture2D.Apply();
    //                byte[] png = texture2D.EncodeToPNG();
    //                Destroy(texture2D);
    //                File.WriteAllBytes(Application.dataPath + "/Resources/Images/Test.png", png);
    //                WWW www = null;
    //                AssetDatabase.Refresh();
    //            }
    //        }
    //        else if (GUILayout.Button("Play",gUILayoutOptions))
    //        {
    //            if (webCamTexture.isPlaying == false)
    //            {
    //                Debug.Log("Call");
    //                webCamTexture.Play();
    //                display.texture = webCamTexture;
    //            }
    //        }
    //    }
}

