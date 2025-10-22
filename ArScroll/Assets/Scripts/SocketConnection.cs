using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public class SocketConnection : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;
    private Coroutine currentCoroutine;

    private string serverIP = "127.0.0.1"; //replace server device ip for mobile app testing 
    private int serverPort = 65432;

    private string dir;
    private string[] sectionList = { "description", "process", "symbolism", "history", "related_works" };
    private int sectionIndex = 0;

    public void Start()
    {
        // PythonRunner.RunFile($"{Application.dataPath}/Scripts/DescPrint.py");
        dir = Application.persistentDataPath + "/GeneratedTexts/";
        ConnectToPythonServer();
    }

    public void SendImageData(string imageData)
    {
        SendMessageToPython(imageData);
    }

    public void ConnectToPythonServer()
    {
        print("Attempting connection to server: " + serverIP + " port:" + serverPort);
        try
        {
            client = new TcpClient(serverIP, serverPort);
            stream = client.GetStream();
            print("Connected to Python server.");

            // Start a new thread for receiving data
            receiveThread = new Thread(ReceiveData);
            receiveThread.IsBackground = true;
            receiveThread.Start();

            // SendMessageToPython("Hello from Unity!");
        }
        catch (SocketException e)
        {
            Debug.LogError("SocketException: " + e.ToString());
        }
    }

    void SendMessageToPython(string message)
    {
        if (stream != null && stream.CanWrite)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Debug.Log("Sent to Python: " + message);
        }
    }

    void ReceiveData()
    {
        byte[] buffer = new byte[2000];
        bool generation = false;
        while (client.Connected)
        {
            try
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Debug.Log("Received from Python: " + receivedMessage);

                    if (generation)
                    {
                        //repeats till "generation done" is passed in
                        string imageName = GlobalInfo.names[GlobalInfo.names.Count - 1];
                        
                        string path = dir + "/" + imageName + "_" + sectionList[sectionIndex] + ".txt";
                        File.AppendAllText(path, receivedMessage);

                        sectionIndex++;
                        
                        if(sectionIndex > 4) // finished with iterating through sections list
                        {
                            generation = false;
                            sectionIndex = 0;
                            OpenBox.loadDone();
                        }
                    }
                    if (receivedMessage == "Generating text")
                    {
                        generation = true;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error receiving data: " + e.Message);
                break;
            }
        }
    }

    void OnApplicationQuit()
    {
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }
        if (client != null)
        {
            client.Close();
        }
    }
}