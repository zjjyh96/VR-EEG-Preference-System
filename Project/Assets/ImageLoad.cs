using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ImageLoad : MonoBehaviour
{
    public string url;
    public RawImage image;
    void Start(){
    }

    public void UpdateImage(string loaclPath)
    {
        // load texture from resource folder
        Texture2D myTexture = Resources.Load(loaclPath) as Texture2D;
        image.texture = myTexture;
    }

    //IEnumerator DownloadImage(string MediaUrl)
    //{   
    //    UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
    //    yield return request.SendWebRequest();
    //    if(request.isNetworkError || request.isHttpError) 
    //        Debug.Log(request.error);
    //    else
    //        image.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
    //} 

}
