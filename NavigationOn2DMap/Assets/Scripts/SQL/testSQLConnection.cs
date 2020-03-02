using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using MySql.Data;
using System;

public class testSQLConnection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string connStr = "server=unitynavigation.servebeer.com;user=Navigator;database=UnityNavigation;port=32261;password=compass123";
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            print("Connecting to MySQL...");
            conn.Open();

            string sql = "SELECT * FROM Test";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                print(rdr[0] + " -- " + rdr[1]);
            }
            rdr.Close();
        }
        catch (Exception ex)
        {
            print(ex.ToString());
        }

        conn.Close();
        print("Done.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
