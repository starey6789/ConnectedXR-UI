using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using static MenuMetrics;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class UIManager : MonoBehaviour
{
    public UIDocument uiDocument;

    // Counter to see what page we're on
    private int counter;

    // These are the UI elements
    // Pages
    private VisualElement menu, startMenu, backMenu, rightMenu;

    // String to see status of menu
    private static String status;

    // Metrics for time collection
    private static float startTime, endTime, totalTime;
    private static float menuStartTime, menuEndTime, menuTotalTime;
    private static bool descToggle = true; // boolean variable for status of home menu having description or history expanded
    void Start()
    {
        //Set counter to 0 (EX: Page 0). This is to keep track of where the Back button goes.
        counter = 0;

        // Set it so that ONLY the first right arrow is on the UI
        displayNone("menu");
        displayNone("backMenu");
        displayNone("rightMenu");
        displayFlex("startMenu");
    }

    void OnEnable()
    {
        // metrics collection
        MenuMetrics.IncrementClick("Symbolism");
        menuStartTime = Time.time;
        startTime = menuStartTime;

        // This is every component of the UI, set references to each
        var root = uiDocument.rootVisualElement;

        menu = root.Q<VisualElement>("MenuButtons");
        startMenu = root.Q<VisualElement>("ArrowForward");
        backMenu = root.Q<VisualElement>("BackButtons");
        rightMenu = root.Q<VisualElement>("RightButtons");
    }

    // load text given from a file
    public string LoadTextFromFile(string fileName)
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

    //populate UI
    public void populate(String contentName, String paintingName, Label header, Label text)
    {

        header.text = contentName;
        text.text = LoadTextFromFile(("file" + contentName + paintingName));
    }

    //Metric functions
    public static void TimeLog(String page)
    {
        switch (page)
        {
            case "exit home": // exiting home
                menuEndTime = Time.time;
                endTime = menuEndTime;
                totalTime = endTime - startTime;
                menuTotalTime = menuEndTime - menuStartTime;

                MenuMetrics.LogMetrics(status, menuStartTime, menuEndTime, menuTotalTime);
                if (descToggle) // is on description
                {
                    MenuMetrics.LogMetrics("Description", startTime, endTime, totalTime);
                }
                else
                {
                    MenuMetrics.LogMetrics("History", startTime, endTime, totalTime);
                }
                totalTime = 0;
                menuTotalTime = 0;
                break;

            case "exit module menus": // symbolism, related works, process
                endTime = Time.time;
                totalTime = endTime - startTime;
                MenuMetrics.LogMetrics(status, startTime, endTime, totalTime);
                totalTime = 0;
                startTime = Time.time;
                break;

            case "enter module menus":
                endTime = Time.time;
                totalTime = endTime - startTime;
                if (descToggle) // is on description
                {
                    MenuMetrics.LogMetrics("Description", startTime, endTime, totalTime);
                }
                else
                {
                    MenuMetrics.LogMetrics("History", startTime, endTime, totalTime);
                }
                totalTime = 0;
                break;

            case "hide":
                menuEndTime = Time.time;
                totalTime = menuEndTime - startTime;
                menuTotalTime = menuEndTime - menuStartTime;

                MenuMetrics.LogMetrics(status, startTime, endTime, totalTime);
                MenuMetrics.LogMetrics("Home", menuStartTime, menuEndTime, menuTotalTime);
                break;
        }
    }

    // method for ImageStatusTracker to call regarding active state of menu 
    public static void ActiveStateLog(bool activeState)
    {
        if (activeState) // prefab is active, called in ImageStatusTracker
        {
            menuStartTime = Time.time;
            startTime = menuStartTime;
        }
        else
        {
            TimeLog("hide");
        }
    }

    //Reset to all the way back
    public void ResetView()
    {
        if (counter >= 2)
        {
            Debug.Log(status);
            menu.style.display = DisplayStyle.Flex;
            backMenu.style.display = DisplayStyle.Flex;
            rightMenu.style.display = DisplayStyle.None;
            startMenu.style.display = DisplayStyle.None;
            counter--;

            // endTime = Time.time;
            // totalTime = endTime - startTime;
            // MenuMetrics.LogMetrics(status, startTime, endTime, totalTime);
            // totalTime = 0;
            // status = "Home";
            TimeLog("exit module menus");
            SetStatus("Home");
        }
        else
        {
            Debug.Log("toggle off");
            displayNone("menu");
            displayNone("backMenu");
            displayNone("rightMenu");
            displayFlex("startMenu");

            DecrementCounter();

            TimeLog("exit home");
        }
    }

    // First Page of the UI
    public void EnterView()
    {
        displayFlex("menu");
        displayFlex("backMenu");
        displayNone("rightMenu");
        displayNone("startMenu");
   
        //increment counter (EX: page 1) and sets status
        IncrementCounter();
        SetStatus("Home");
    }

    // Set status of what page currently on
    public static void SetStatus(String state)
    {
        status = state;
    }

    //Increment counter
    public void IncrementCounter()
    {
        counter++;
    }
    public void DecrementCounter()
    {
        counter--;
    }

    // Set startTime
    public static void setStartTime()
    {
        startTime = Time.time;
    }

    // Display Page None
    public void displayNone(String whatMenu)
    {
        switch (whatMenu)
        {
            case "menu":
                menu.style.display = DisplayStyle.None;
                break;
            case "backMenu":
                backMenu.style.display = DisplayStyle.None;
                break;
            case "rightMenu":
                rightMenu.style.display = DisplayStyle.None;
                break;
            case "startMenu":
                startMenu.style.display = DisplayStyle.None;
                break;
            default:
                break;
        }
    }

    // Display Page Flex
   public void displayFlex(String whatMenu)
    {
        switch (whatMenu)
        {
            case "menu":
                menu.style.display = DisplayStyle.Flex;
                break;
            case "backMenu":
                backMenu.style.display = DisplayStyle.Flex;
                break;
            case "rightMenu":
                rightMenu.style.display = DisplayStyle.Flex;
                break;
            case "startMenu":
                startMenu.style.display = DisplayStyle.Flex;
                break;
            default:
                break;
        }
    }

    public static void DescriptionToggleTrue()
    {
        descToggle = true;
    }

    public static void DescriptionToggleFalse()
    {
        descToggle = true;
    }

}
