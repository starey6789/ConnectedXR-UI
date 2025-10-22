using UnityEngine;
using System.Collections;
using System.IO;

public class CleanUp : MonoBehaviour
{
    [SerializeField]
    private GameObject cleanButton;

    public void cleanUp() //implement button for quitting or a reset cache system
    {
        Debug.Log("Clean up time...!");

        string[] filePaths = Directory.GetFiles(Application.persistentDataPath + "/GeneratedTexts/");
        foreach (string file in filePaths)
        {
            Debug.Log(file + " to be deleted");
            
            File.Delete(file);
        }
        Debug.Log("Done with cleanup!");
    }
    
}
