using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Profiling;
using System.IO;

public class DownloadTextureTest : MonoBehaviour
{
    [SerializeField]
    string url = null;

    [SerializeField]
    string path = null;

    [SerializeField]
    SpriteRenderer sp0 = null;

    [SerializeField]
    SpriteRenderer sp1 = null;

    IEnumerator DownloadTextureByUnityWebRequest(string url)
    {
        var rq = UnityWebRequestTexture.GetTexture(url, true);
        yield return rq.SendWebRequest();

        Profiler.BeginSample("////// UnityWebRequest");
        Texture2D tex = DownloadHandlerTexture.GetContent(rq);
        Profiler.EndSample();

        Debug.Log($"WebRequest {tex.mipmapCount}");
        Debug.Log(tex.format);
        Debug.Log(Time.frameCount);

        sp0.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
    }

    IEnumerator DownloadTextureByWWW(string url)
    {
        var www = new WWW(url);
        yield return www;

        Profiler.BeginSample("////// WWW");
        Texture2D tex = www.textureNonReadable;
        Profiler.EndSample();

        Debug.Log($"WWW {tex.mipmapCount}");
        Debug.Log(tex.format);
        Debug.Log(Time.frameCount);

        sp1.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
    }

    void LoadImage(string path)
    {
        var tex = new Texture2D(16, 16, TextureFormat.RGB24, false);
        Debug.Log(tex.mipmapCount);

        byte[] bytes = File.ReadAllBytes(path);

        tex.LoadImage(bytes, true);

        Debug.Log(tex.mipmapCount);
    }

    void Start()
    {
        StartCoroutine(DownloadTextureByUnityWebRequest(url));
        StartCoroutine(DownloadTextureByWWW(url));

        LoadImage(path);
    }

}
