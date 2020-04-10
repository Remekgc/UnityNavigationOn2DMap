using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleSQLUserScriptExample : MonoBehaviour
{
    public SQLManager sqlManager;
    public string data;
    public TextMeshProUGUI SQLText;

    public void GetDatabaseData()
    {
        StartCoroutine(IGetDatabaseData());
    }

    public IEnumerator IGetDatabaseData()
    {
        Debug.Log("IGetDatabaseData started");  
        sqlManager.ExecuteReaderQuery("SELECT Name FROM Building WHERE Name = 'Adrian_Home'");
        while (true)
        {
            if (sqlManager.SelectQueryDone)
            {
                data = sqlManager.SelectQueryResult;
                SQLText.text = data;
                yield break;
            }
            else
            {
                print("Waiting suka!");
                yield return new WaitForSecondsRealtime(2);
            }
        }

    }

}
