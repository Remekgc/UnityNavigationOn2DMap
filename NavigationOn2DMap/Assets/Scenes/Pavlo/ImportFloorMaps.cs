using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImportFloorMaps : MonoBehaviour
{
    Vector3 NORMAL_SCALE = new Vector3(1, 1, 1);
    [SerializeField] List<string> listOfMapsLinks;
    [SerializeField] List<string> listOfMapsPaths;
    string imageLink = "https://drive.google.com/file/d/1CJ7QxTPCnQ_ne9n6Kl9TH8ANmuf5lIQt/view?usp=sharing";
    string imageLink2 = "https://drive.google.com/file/d/1aPNeii8yoJEyep3qJgi-_fPTQBu0DcRx/view?usp=sharing";
    string buildingName = "WSEI";
    string fileName;
    string appPath;
    string folderPath;
    string currentFilePath;
    string currentImageLink = null;
    [SerializeField] Button button;
    [SerializeField] GameObject contentList;
    float progress;
    Texture2D tempTexture;
    [SerializeField] Image testImage;
    Coroutine importImage;
    Coroutine showImageCoroutine;
    GameObject tempObj;
    string currentReadyLink;
    int id = 0;

    private void Start()
    {
        listOfMapsLinks.Add(imageLink);
        listOfMapsLinks.Add(imageLink2);
        appPath = Path.GetDirectoryName(Application.persistentDataPath);
        folderPath = Path.Combine(appPath, buildingName);
        CheckForFolder();
    }

    private void CreateButtons()
    {
        int imageNum = 0;
        foreach (string mapString in listOfMapsLinks)
        {
            Button instantiatedButton = Instantiate(button);
            instantiatedButton.name = imageNum + "_floorMap";
            instantiatedButton.GetComponentInChildren<TextMeshProUGUI>().text = imageNum + " Floor map";
            instantiatedButton.transform.SetParent(contentList.transform);
            instantiatedButton.transform.localScale = NORMAL_SCALE;
            instantiatedButton.GetComponent<Button>().onClick.AddListener(ShowImage);
            imageNum++;
        }
        imageNum = 0;
    }

    private void CheckForFolder()
    {
        if (Directory.Exists(folderPath))
        {
            CheckForMaps();
        }
        else
        {
            Directory.CreateDirectory(folderPath);
            CheckForMaps();
        }
    }

    private void CheckForMaps()
    {

        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        print(directoryInfo);
        FileInfo[] folderFiles = directoryInfo.GetFiles("*.*");

        int fileCounter = 0;
        foreach (FileInfo file in folderFiles)
        {
            if (file.Extension == ".png")
                fileCounter++;
            listOfMapsPaths.Add(Path.GetFullPath(file.ToString()));
        }
        if(fileCounter == listOfMapsLinks.Count)
        {
            fileCounter = 0;
            CreateButtons();
        }
        else
        {
            foreach (FileInfo file in folderFiles)
            {
                File.Delete(file.ToString());

            }
            importImage = StartCoroutine(DownloadImage());
        }
    }

    IEnumerator DownloadImage()
    {
        foreach( string link in listOfMapsLinks)
        {
            string imageId = link.Split('/')[5];
            currentReadyLink = "https://drive.google.com/uc?id=" + imageId;
            string currentFileName = id + " Floor.png";
            currentFilePath = Path.Combine(folderPath, currentFileName);
            listOfMapsPaths.Add(currentFilePath);

            UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(currentReadyLink);
            var operation = imageRequest.SendWebRequest();

            while (!imageRequest.isDone)
            {
                progress = operation.progress;
                yield return null;
            }
            progress = 1f;
            Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
            byte[] bytes = texture.EncodeToPNG();
            print(currentFilePath);
            File.WriteAllBytes(currentFilePath, bytes);

            Button instantiatedButton = Instantiate(button);
            instantiatedButton.name = id + "_floorMap";
            instantiatedButton.GetComponentInChildren<TextMeshProUGUI>().text = id + " Floor map";
            instantiatedButton.transform.SetParent(contentList.transform);
            instantiatedButton.transform.localScale = NORMAL_SCALE;
            instantiatedButton.GetComponent<Button>().onClick.AddListener(ShowImage);

            id++;
            importImage = null;
        }   
    }

    private void ShowImage()
    {
        showImageCoroutine = StartCoroutine(OpenFloorMap());
    }
    IEnumerator OpenFloorMap()
    {
        int index = int.Parse(EventSystem.current.currentSelectedGameObject.name.Split('_')[0]);
        print(index);
        if (!testImage.IsActive())
        {
            WWW www = new WWW(listOfMapsPaths[index]);
            yield return www;
            Texture2D tempTexture = www.texture;
            testImage.sprite = Sprite.Create(tempTexture, new Rect(x: 0, y: 0, tempTexture.width, tempTexture.height), new Vector2(x: 0, y: 0));
            //var objTransform = testImage.transform as RectTransform;
            //objTransform.sizeDelta = new Vector2(tempTexture.width, tempTexture.height);
            testImage.gameObject.SetActive(true);
        }
        else
        {
            WWW www = new WWW(listOfMapsPaths[index]);
            yield return www;
            Texture2D tempTexture = www.texture;
            testImage.sprite = Sprite.Create(tempTexture, new Rect(x: 0, y: 0, tempTexture.width, tempTexture.height), new Vector2(x: 0, y: 0));
            //var objTransform = testImage.transform as RectTransform;
            //objTransform.sizeDelta = new Vector2(tempTexture.width, tempTexture.height);
        }
        showImageCoroutine = null;
    }
}
