using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using TMPro;
using System.Threading;

public class testSQLConnection : MonoBehaviour
{
    public TextMeshProUGUI SQLText;
    public string data;
    private bool threadDone = false;

    void Start()
    {
        var testThread = new Thread(getData);
        data = "Connecting to SQL";
        SQLText.text += data;
        testThread.Start();
    }
    public void getData()
    {
        string connStr = "server=unitynavigation.servebeer.com;user=Navigator;database=UnityNavigation;port=32261;password=compass123";
        MySqlConnection conn = new MySqlConnection(connStr);
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
