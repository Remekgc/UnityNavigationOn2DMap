using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleSQLUserScriptExample : MonoBehaviour
{
    public SQLManager sqlManager;
    public string data;
    public TextMeshProUGUI SQLText;
    public string sqlQuery;
    public void GetDatabaseData()
    {
        StartCoroutine(IGetDatabaseData());
    }

    public IEnumerator IGetDatabaseData()
    {
        Debug.Log("IGetDatabaseData started");  
        sqlManager.ExecuteReaderQuery(sqlQuery);
        while (true)
        {
            if (sqlManager.selectQueryDone)
            {
                //data = sqlManager.selectQueryResult;
                data = sqlManager.selectQueryResult[0][1].ToString();
                SQLText.text = data;
                yield break;
            }
            else
            {
                print("Query not finished yet");
                yield return new WaitForSecondsRealtime(2);
            }
        }

    }

}
