using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImportNewImage : MonoBehaviour
{
    string imageLink = "https://drive.google.com/file/d/1CJ7QxTPCnQ_ne9n6Kl9TH8ANmuf5lIQt/view?usp=sharing";
    int imageNum = 0;
    float progress;
    [SerializeField] Image testImage;
    Coroutine importImage;

    private void Start()
    {
        importImage = StartCoroutine(Import(imageLink));
    }

    private void Update()
    {
        print(PlayerPrefs.GetString("Map1"));
    }

    IEnumerator Import(string link)
    {
        imageNum++;
        string imageId = link.Split('/')[5];
        string readyLink = "https://drive.google.com/uc?id=" + imageId;
        PlayerPrefs.SetString("Map" + imageNum, readyLink);
        print(imageId);
        UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(readyLink);
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
