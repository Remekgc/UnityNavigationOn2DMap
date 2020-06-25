using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using TMPro;
using System.Threading;

public class SQLManager : MonoBehaviour
{
    private string ConnStr = "server=unitynavigation.servebeer.com;" +
                            "user=Navigator;" +
                            "database=UnityNavigation;" +
                            "port=32261;" +
                            "password=compass123";
    private Thread selectQueryThread;

    public bool threadDone = false, selectQueryDone = true, emptyQueryResult = false;
    [SerializeField] protected TextMeshProUGUI SQLText;
    public string testData;

    // public Thread parameters - Change thouse only at the end in other thread to make the result public.
    public List<List<string>> selectQueryResult = new List<List<string>>();

    // private Thread parameters - Change thouse only in other thread.
    private string queryStatement;
    private List<List<string>> _selectQueryResult = new List<List<string>>();

    public void ExecuteReaderQuery(string sqlQuery)
    {
        if (selectQueryThread != null && selectQueryThread.IsAlive) // Checking if thread is defined and alive
        {
            Debug.Log("Query already executing, please wait for the result before running again.");
        }
        else // Otherwise creating a new thread and running it
        {
            Debug.Log("Starting new sql query.");
            queryStatement = sqlQuery;
            emptyQueryResult = false;
            selectQueryDone = false;
            selectQueryThread = new Thread(ReaderQuery);
            selectQueryThread.Start();
        }
    }

    public void ReaderQuery()
    {
        MySqlConnection sqlConnection = new MySqlConnection(ConnStr); // Creating sql connection with defined connection data
        try
        {
            sqlConnection.Open();
            MySqlCommand cmd = new MySqlCommand(queryStatement, sqlConnection); // Creating new sql qurey command
            MySqlDataReader dataReader = cmd.ExecuteReader();

            selectQueryResult.Clear();
            _selectQueryResult.Clear();

            int listID = 0;

            while (dataReader.Read())
            {
                _selectQueryResult.Add(new List<string>());
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    _selectQueryResult[listID].Add(dataReader[i].ToString());
                }
                listID++;
            }
            dataReader.Close();

            if (listID == 0)
            {
                emptyQueryResult = true;
                throw new EmptyResult();
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Reader Querry error: " + ex);
        }
        selectQueryResult = _selectQueryResult;
        selectQueryDone = true;
    }

    void Update()
    {
        if (threadDone)
        {
            SQLText.text = testData;
            threadDone = !threadDone;
        }
    }
}
