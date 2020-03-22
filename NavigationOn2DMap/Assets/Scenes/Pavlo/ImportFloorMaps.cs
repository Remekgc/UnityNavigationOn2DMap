using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImportFloorMaps : MonoBehaviour
{
    Vector3 NORMAL_SCALE = new Vector3(1, 1, 1);
    [SerializeField] List<string> listOfMapsLinks;
    [SerializeField] List<Texture2D> listOfMapsImages;
    string imageLink = "https://drive.google.com/file/d/1CJ7QxTPCnQ_ne9n6Kl9TH8ANmuf5lIQt/view?usp=sharing";
    string imageLink2 = "https://drive.google.com/file/d/1aPNeii8yoJEyep3qJgi-_fPTQBu0DcRx/view?usp=sharing";
    string currentImageLink = null;
    [SerializeField] Button button;
    [SerializeField] GameObject contentList;
    int imageNum = 0;
    float progress;
    Texture2D tempTexture;
    [SerializeField] Image testImage;
    Coroutine importImage;

    private void Start()
    {
        listOfMapsLinks.Add(imageLink);
        listOfMapsLinks.Add(imageLink2);
        foreach (string mapString in listOfMapsLinks)
        {
            
            currentImageLink = listOfMapsLinks[imageNum];
            print(imageNum);
            print(currentImageLink);
            Button instantiatedButton = Instantiate(button);
            instantiatedButton.name = imageNum + "_floorMap";
            instantiatedButton.GetComponentInChildren<TextMeshProUGUI>().text = imageNum + " Floor map";
            instantiatedButton.transform.SetParent(contentList.transform);
            instantiatedButton.transform.localScale = NORMAL_SCALE;
            instantiatedButton.GetComponent<Button>().onClick.AddListener(OpenFloorMap);
            importImage = StartCoroutine(Import(currentImageLink));
        }
        imageNum = 0;
    }

    private void OpenFloorMap()
    {
        int index = int.Parse(EventSystem.current.currentSelectedGameObject.name.Split('_')[0]);
        if (!testImage.IsActive())
        {
            tempTexture = listOfMapsImages[index];
            testImage.sprite = Sprite.Create(tempTexture, new Rect(x: 0, y: 0, tempTexture.width, tempTexture.height), new Vector2(x: 0, y: 0));
            var objTransform = testImage.transform as RectTransform;
            objTransform.sizeDelta = new Vector2(tempTexture.width, tempTexture.height);
            testImage.gameObject.SetActive(true);
        }
        else
        {
            tempTexture = listOfMapsImages[index];
            testImage.sprite = Sprite.Create(tempTexture, new Rect(x: 0, y: 0, tempTexture.width, tempTexture.height), new Vector2(x: 0, y: 0));
            var objTransform = testImage.transform as RectTransform;
            objTransform.sizeDelta = new Vector2(tempTexture.width, tempTexture.height);
        }
        
        
    }

    IEnumerator Import(string link)
    {
        
        string imageId = link.Split('/')[5];
        string readyLink = "https://drive.google.com/uc?id=" + imageId;
        PlayerPrefs.SetString("Map" + imageNum, readyLink);
        imageNum++;
        UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(readyLink);
        var operation = imageRequest.SendWebRequest();

        while (!imageRequest.isDone)
        {
            progress = operation.progress;
            yield return null;
        }
        progress = 1f;
        Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
        listOfMapsImages.Add(texture);
        
        currentImageLink = null;
        importImage = null;
    }
}
