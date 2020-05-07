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

    public bool threadDone = false, SelectQueryDone = true;
    [SerializeField] protected TextMeshProUGUI SQLText;
    public string testData, QueryStatement;
    public List<List<string>> selectQueryResult = new List<List<string>>();

    void Start()
    {
        /*var testThread = new Thread(getData);
        data = "Connecting to SQL";
        SQLText.text += data;
        testThread.Start();*/
        //ExecuteReaderQuery("SELECT Name FROM Building WHERE Name = 'Adrian_Home'");
    }

    public void ExecuteReaderQuery(string sqlQuery)
    {
        if (selectQueryThread != null && selectQueryThread.IsAlive) // Checking if thread is defined and alive
        {
            print("Query already executing, please wait for the result before running again.");
        }
        else // Otherwise creating a new thread and running it
        {
            SelectQueryDone = false;
            QueryStatement = sqlQuery;
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
            MySqlCommand cmd = new MySqlCommand(QueryStatement, sqlConnection); // Creating new sql qurey command
            MySqlDataReader dataReader = cmd.ExecuteReader();

            selectQueryResult.Clear();
            int listID = 0;

            while (dataReader.Read())
            {
                selectQueryResult.Add(new List<string>());
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    selectQueryResult[listID].Add(dataReader[i].ToString());
                }
                listID++;
            }
            dataReader.Close();
        }
        catch (Exception ex)
        {
            print("Reader Querry error: " + ex);
        }
        SelectQueryDone = true;
    }

    // Test Fuction - not important
    private void GetTestData()
    {
        MySqlConnection conn = new MySqlConnection(ConnStr);
        try
        {
            //print("Connecting to MySQL...");
            conn.Open();

            string sql = "SELECT * FROM Test";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            testData = "";
            while (rdr.Read())
            {
                //print(rdr[0] + " -- " + rdr[1]);
                testData += rdr[0] + " " + rdr[1] + "\n";
            }
            rdr.Close();
        }
        catch
        {
            testData = "Failed to connect";
        }

        conn.Close();
        //print("Done.");
        threadDone = !threadDone;
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
