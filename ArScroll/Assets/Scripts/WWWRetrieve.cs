using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class WWWRetrieve : MonoBehaviour
{
    public string fileName = "PutFileNameHere"; // Without .txt

    public string LoadTextFromFile()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
        if (textAsset != null)
        {
            return textAsset.text;
        }
        else
        {
           return "Could not find file: " + fileName;
        }
    }


   
}
