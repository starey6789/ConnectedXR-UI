using UnityEngine;
using System.Diagnostics;

public class PythonRunner : MonoBehaviour
{
    private Process pythonProcess;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pythonProcess = Process.Start("python", @"Assets\Scripts\DescPrint.py");
        UnityEngine.Debug.Log("Python script started.");
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnApplicationQuit()
    {
        // Clean up resources if needed
        if (pythonProcess != null && !pythonProcess.HasExited)
        {
            pythonProcess.Kill();
            pythonProcess.Dispose();
        }
    }
}
