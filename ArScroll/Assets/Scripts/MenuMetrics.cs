// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.IO;
// using System.Text.Json;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;


class MenuMetrics
{
    static string dir = Path.Combine(Application.persistentDataPath, "logs");

    /*static void Main(string[] args)
    {
        LogMetrics("MENU3", 333, (float)4.20, 2);
        LogMetrics("MENU3", 2, (float)855.2, 5);
        LogMetrics("MENU6", 2, (float)88, 4);
        LogMetrics("MENU1", 24, (float)69.99, 5);
    } */

    //logs the starting timestamp, ending timestamp, time spent
    //into a json file with the name of the menu
    //will increase the counter by 1
    public static void LogMetrics(string name, float startTime, float endTime, float timespent)
    {
        //path of the menu's json file
        string FileName = name + ".json";
        string path = Path.Combine(dir, FileName);

        // Debug.Log("pre-appending");

        //initialize a new menu or 
        //a menu with existing data loaded in
        ArtMenu menu = Initialize(path);
        // Debug.Log("menu object created");
        menu.Timestamps.Add(startTime);
        // Debug.Log("first add done");
        menu.Timestamps.Add(endTime);
        menu.Timespent.Add(timespent);
        menu.LookCounter += 1;
        menu.TotalTime += timespent;
        // Debug.Log("starting append");
        Append(menu, path);
        // Debug.Log("appended");
    }
    
    public static void IncrementClick(string name)
    {
        //path of the menu's json file
        string FileName = name + ".json";
        string path = Path.Combine(dir, FileName);

        //initialize a new menu or 
        //a menu with existing data loaded in
        ArtMenu menu = Initialize(path);

        menu.ClickCounter += 1;
        
        Append(menu, path);
        
    }

    //creates the directory and/or json file if it does not exist
    //if the file does exist, read from it
    //returns ArtMenu item read from the json file with existing data
    //or a new ArtMenu item
    static ArtMenu Initialize(string path)
    {
        //makes directory
        FileInfo fileInfo = new FileInfo(dir);
        if (!fileInfo.Exists)
        {
            Directory.CreateDirectory(dir);
        }

        //creates json file
        if (!File.Exists(path))
        {
            File.Create(path).Close();
            return new ArtMenu();
        }

        else //file exists
        {
            string existingJSON = File.ReadAllText(path);
            if (existingJSON == null)
            {
                return new ArtMenu();
            }
            ArtMenu? m = JsonUtility.FromJson<ArtMenu>(existingJSON);

            if (m == null)
            {
                return new ArtMenu();
            }
            return m;
        }

    }

    //Appends new data to json file by overwriting the file
    //with the existing data and new data
    static void Append(ArtMenu m, string path)
    {
        // var options = new JsonUtility();
        // options.PropertyNameCaseInsensitive = true;
        // options.WriteIndented = true;

        string data = JsonUtility.ToJson(m, true);
        File.WriteAllText(path, data);
    }
}

    [Serializable]
    public class ArtMenu
    {
        public int LookCounter = 0;
        public int ClickCounter = 0;
        public float TotalTime = 0;
        public List<float> Timespent = new List<float>();
        public List<float> Timestamps = new List<float>();
    }
/*public class ArtMenu
{

    public ArtMenu()
    {
        Timespent = new List<float>();
        Timestamps = new List<float>();
    }

    public List<float> Timespent { get; set; }

    public List<float> Timestamps { get; set; }
    public int Counter { get; set; }
}*/

