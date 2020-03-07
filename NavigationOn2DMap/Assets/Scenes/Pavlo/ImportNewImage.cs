using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImportNewImage : MonoBehaviour
{
    string imageLink = "https://www.w3schools.com/w3css/img_snowtops.jpg";
    float progress;
    [SerializeField] Image testImage;
    Coroutine importImage;

    private void Start()
    {
        importImage = StartCoroutine(Import(imageLink));
    }

    IEnumerator Import(string link)
    {
        UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(link);
        var operation = imageRequest.SendWebRequest();

        while (!imageRequest.isDone)
        {
            progress = operation.progress;
            yield return null;
        }
        progress = 1f;
        Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
        testImage.gameObject.SetActive(true);
        testImage.sprite = Sprite.Create(texture, new Rect(x: 0, y: 0, texture.width, texture.height), new Vector2(x: 0, y: 0));
        testImage.SetNativeSize();
        importImage = null;
    }
}
