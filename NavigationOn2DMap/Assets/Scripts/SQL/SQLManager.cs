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
    private bool threadDone = false, QueryTreadStatus = false;
    private int numberOfValuesForSorting = 0;

    public TextMeshProUGUI SQLText;
    public string data, QueryStatement, QueryResult;

    void Start()
    {
        /*var testThread = new Thread(getData);
        data = "Connecting to SQL";
        SQLText.text += data;
        testThread.Start();*/
        ExecuteReaderQuery("SELECT Name FROM Building WHERE Name = 'Adrian_Home'", 1);
    }

    public void ExecuteReaderQuery(string sqlQuery, int numberOfValuesSelected)
    {
        numberOfValuesForSorting = numberOfValuesSelected;
        QueryStatement = sqlQuery;
        var queryThread = new Thread(ReaderQuery);
        if (queryThread.IsAlive)
        {
            print("Query already executing, please wait for the result before running again.");
        }
        else
        {
            queryThread.Start();
        }
    }

    public void ReaderQuery()
    {
        MySqlConnection sqlConnection = new MySqlConnection(ConnStr);
        try
        {
            sqlConnection.Open();
            MySqlCommand cmd = new MySqlCommand(QueryStatement, sqlConnection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            QueryResult = "";
            while (dataReader.Read())
            {
                print(dataReader[0]);
            }
            dataReader.Close();
        }
        catch
        {
            print("Reader Querry error");
        }
    }

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

            data = "";
            while (rdr.Read())
            {
                //print(rdr[0] + " -- " + rdr[1]);
                data += rdr[0] + " " + rdr[1] + "\n";
            }
            rdr.Close();
        }
        catch
        {
            data = "Failed to connect";
        }

        conn.Close();
        //print("Done.");
        threadDone = !threadDone;
    }

    void Update()
    {
        if (threadDone)
        {
            SQLText.text = data;
            threadDone = !threadDone;
        }
    }
}
