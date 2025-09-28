using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SocketConnection : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;

    public string serverIP = "127.0.0.1";
    public int serverPort = 65432;

    public void Start()
    {
        ConnectToPythonServer();
    }

    public void SendImageData(string imageData)
    {
        SendMessageToPython(imageData);
    }

    public void ConnectToPythonServer()
    {
        try
        {
            client = new TcpClient(serverIP, serverPort);
            stream = client.GetStream();
            Debug.Log("Connected to Python server.");

            // Start a new thread for receiving data
            receiveThread = new Thread(ReceiveData);
            receiveThread.IsBackground = true;
            receiveThread.Start();

            SendMessageToPython("Hello from Unity!");
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
                    Debug.Log("Received from Python: " + receivedMessage);
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