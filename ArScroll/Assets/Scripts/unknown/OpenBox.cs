using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static MenuMetrics;

// This script works to give function to the entire menu.
// If you want to edit:
//                  Metric/Data collections
//                  The way the menu functions

public class OpenBox : MonoBehaviour
{
    public UIDocument uiDocument;

    // These are the text files to populate the boxes
    [SerializeField] string fileDesc, fileHistory, fileTitle, fileSymbolism, fileWorks, fileTechniques;

    // These are the UI elements
    // Pages
    private VisualElement menu, startMenu, backMenu, rightMenu;

    // Containers
    private Button nextButton, backButton, symbolButton, relatedButton, processButton;
    private ScrollView descriptionContainer, historyContainer, titleContainer;

    // Text Headers
    private Label descriptionHeader, historyHeader, titleHeader, rightHeader, descriptionText, historyText, titleText, rightText;

    // Counter to see what page we're on
    private int counter;
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
        menu.style.display = DisplayStyle.None;
        backMenu.style.display = DisplayStyle.None;
        rightMenu.style.display = DisplayStyle.None;
        startMenu.style.display = DisplayStyle.Flex;

        //populate contents so its not left with placeholder text
        populate();
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

        nextButton = root.Q<Button>("Forward");
        backButton = root.Q<Button>("Back");
        descriptionContainer = root.Q<ScrollView>("Desc");
        historyContainer = root.Q<ScrollView>("History");
        titleContainer = root.Q<ScrollView>("Title");
        symbolButton = root.Q<Button>("Symbol");
        relatedButton = root.Q<Button>("Related");
        processButton = root.Q<Button>("Process");

        descriptionHeader = root.Q<Label>("DescHeader");
        historyHeader = root.Q<Label>("HistoryHeader");
        titleHeader = root.Q<Label>("TitleHeader");
        rightHeader = root.Q<Label>("RightHeader");

        descriptionText = root.Q<Label>("DescText");
        historyText = root.Q<Label>("HistoryText");
        titleText = root.Q<Label>("TitleText");
        rightText = root.Q<Label>("RightText");

        // Add event listeners 
        descriptionContainer.RegisterCallback<ClickEvent>(evt => ExpandDescription());
        historyContainer.RegisterCallback<ClickEvent>(evt => ExpandHistory());

        //If Forward button is pressed, we can enter the menu
        nextButton.clicked += EnterView;
        // If Back button is pressed, we go back to having no menu
        backButton.clicked += ResetView;
        // If One of the buttons on the Right UI is pressed
        symbolButton.clicked += SymbolView;
        relatedButton.clicked += RelatedView;
        processButton.clicked += ProcessView;
    }

    // EVENTS THAT OCCUR WHEN THINGS ARE PRESSED
    // if description box is pressed
    void ExpandDescription()
    {
        //increment metrics click
        MenuMetrics.IncrementClick("Description");
        //toggle status
        TimeLog("enter module menus");

        descToggle = true;

        descriptionContainer.style.height = 330;
        historyContainer.style.height = 100;

        descriptionHeader.text = "Description";
        descriptionText.text = LoadTextFromFile(fileDesc);
    }

    // if history box is pressed

    void ExpandHistory()
    {
        //increment metrics click
        MenuMetrics.IncrementClick("History");
        //toggle status
        TimeLog("enter module menus");
        descToggle = false;
        

        descriptionContainer.style.height = 100;
        historyContainer.style.height = 330;

        historyHeader.text = "History";
        historyText.text = LoadTextFromFile(fileHistory);
    }

    //EVENTS THAT OCCUR FOR SELECTING WHICH UI IS ENABLED
    // First Page of the UI
    void EnterView()
    {
        menu.style.display = DisplayStyle.Flex;
        backMenu.style.display = DisplayStyle.Flex;
        rightMenu.style.display = DisplayStyle.None;
        startMenu.style.display = DisplayStyle.None;

        //increment counter (EX: page 1) and sets status
        counter++;
        status = "Home";
    }

        //Reset to all the way back
    void ResetView()
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
            status = "Home";
        }
        else
        {
            Debug.Log("toggle off");
            menu.style.display = DisplayStyle.None;
            backMenu.style.display = DisplayStyle.None;
            rightMenu.style.display = DisplayStyle.None;
            startMenu.style.display = DisplayStyle.Flex;
            counter--;

            TimeLog("exit home");
        }
    }

    // copy of SymbolView for testing
    void SymbolViewCopy(String hi)
    {
        //increment metrics click
        MenuMetrics.IncrementClick("Symbolism");
        //increment counter (EX: page 2) and sets status
        counter++;
        TimeLog("enter module menus");
        status = "Symbolism";
        startTime = Time.time;

        Debug.Log("symbol");

        //populate text field with Symbol text contents
        rightHeader.text = "Symbolism";
        rightText.text = LoadTextFromFile(fileSymbolism);

            menu.style.display = DisplayStyle.None;
            backMenu.style.display = DisplayStyle.Flex;
            rightMenu.style.display = DisplayStyle.Flex;
            startMenu.style.display = DisplayStyle.None;
    }

    void SymbolView()
    {
        //increment metrics click
        MenuMetrics.IncrementClick("Symbolism");
        //increment counter (EX: page 2) and sets status
        counter++;
        TimeLog("enter module menus");
        status = "Symbolism";
        startTime = Time.time;

        Debug.Log("symbol");

        //populate text field with Symbol text contents
        rightHeader.text = "Symbolism";
        rightText.text = LoadTextFromFile(fileSymbolism);

            menu.style.display = DisplayStyle.None;
            backMenu.style.display = DisplayStyle.Flex;
            rightMenu.style.display = DisplayStyle.Flex;
            startMenu.style.display = DisplayStyle.None;
    }

    void RelatedView()
    {
        //increment metrics click
        MenuMetrics.IncrementClick("Related");
        //increment counter (EX: page 2) and sets status
        counter++;
        TimeLog("enter module menus");
        status = "Related";
        startTime = Time.time;

        //populate text field with Symbol text contents
        rightHeader.text = "Related Works";
        rightText.text = LoadTextFromFile(fileWorks);

        menu.style.display = DisplayStyle.None;
        backMenu.style.display = DisplayStyle.Flex;
        rightMenu.style.display = DisplayStyle.Flex;
        startMenu.style.display = DisplayStyle.None;
    }

    void ProcessView()
    {   
        //increment metrics click
        MenuMetrics.IncrementClick("Process");
        
        //increment counter (EX: page 2) and sets status
        counter++;
        TimeLog("enter module menus");
        status = "Process";
        startTime = Time.time;

        //populate text field with Symbol text contents
        rightHeader.text = "Process";
        rightText.text = LoadTextFromFile(fileTechniques);

            menu.style.display = DisplayStyle.None;
            backMenu.style.display = DisplayStyle.Flex;
            rightMenu.style.display = DisplayStyle.Flex;
            startMenu.style.display = DisplayStyle.None;
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

    // load text given from a fil
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

    //populate UI so it doesn't have null text

    private static void TimeLog(String page)
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
    void populate()
    {
        descriptionHeader.text = "Description";
        descriptionText.text = LoadTextFromFile(fileDesc);

        historyHeader.text = "History";
        historyText.text = LoadTextFromFile(fileHistory);

        titleHeader.text = "Title";
        titleText.text = LoadTextFromFile(fileTitle);
    }
}

