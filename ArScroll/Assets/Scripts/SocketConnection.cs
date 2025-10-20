using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;

public class SocketConnection : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;
    private Coroutine currentCoroutine;

    private string serverIP = "insert your own ip address here";
    private int serverPort = 65432;

    public void Start()
    {
        // PythonRunner.RunFile($"{Application.dataPath}/Scripts/DescPrint.py");
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
        byte[] buffer = new byte[1024];
        while (client.Connected)
        {
            try
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Debug.Log("Received from Python: " + receivedMessage); //look into this and what it prints from the python server
                    // could potentially use this for the trigger to stop loading 
                    if(receivedMessage == "Wrote to file")
                    {
                        OpenBox.loadDone();
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