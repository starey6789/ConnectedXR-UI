using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using static MenuMetrics;
using static UIManager;

// This script has all initializations of the left side of the UI
// If you want to edit:
//                  History
//                  Description
//                  Back Arrow

public class LeftUI : MonoBehaviour
{
    // This UIDocument gets the components in Assets/GUI/Script/
    // Where:
    //  Info is the position of GUI components
    //  ButtonCSS is the CSS styling of the components
    public UIDocument uiDocument;

    // This initializes an object of UIManager
    private UIManager uiManager;

    // These are the text files to populate the boxes
    [SerializeField] string fileDesc, fileHistory;

    // These are the UI elements

    // Containers
    private Button backButton;
    private ScrollView descriptionContainer, historyContainer;

    // Text Headers
    private Label descriptionHeader, historyHeader, descriptionText, historyText;

    void Start()
    {
        //populate contents so its not left with placeholder text
        uiManager.populate("Description", "", descriptionHeader, descriptionText);
        uiManager.populate("History", "", historyHeader, historyText);
  
    }

    void OnEnable()
    {
        // This is every component of the UI, set references to each
        var root = uiDocument.rootVisualElement;

        backButton = root.Q<Button>("Back");
        descriptionContainer = root.Q<ScrollView>("Desc");
        historyContainer = root.Q<ScrollView>("History");
       
        descriptionHeader = root.Q<Label>("DescHeader");
        historyHeader = root.Q<Label>("HistoryHeader");
       
        descriptionText = root.Q<Label>("DescText");
        historyText = root.Q<Label>("HistoryText");

        // Add event listeners 
        descriptionContainer.RegisterCallback<ClickEvent>(evt => ExpandDescription());
        historyContainer.RegisterCallback<ClickEvent>(evt => ExpandHistory());
   
        // If Back button is pressed, we go back to having no menu
        backButton.clicked += uiManager.ResetView;
      
    }

    // EVENTS THAT OCCUR WHEN THINGS ARE PRESSED
    // if description box is pressed
    void ExpandDescription()
    {
        //increment metrics click
        MenuMetrics.IncrementClick("Description");
        //toggle status
        TimeLog("enter module menus");

        DescriptionToggleTrue();

        descriptionContainer.style.height = 330;
        historyContainer.style.height = 100;

        descriptionHeader.text = "Description";
        descriptionText.text = uiManager.LoadTextFromFile(fileDesc);
    }

    // if history box is pressed

    void ExpandHistory()
    {
        //increment metrics click
        MenuMetrics.IncrementClick("History");
        //toggle status
        TimeLog("enter module menus");
        DescriptionToggleFalse();


        descriptionContainer.style.height = 100;
        historyContainer.style.height = 330;

        historyHeader.text = "History";
        historyText.text = uiManager.LoadTextFromFile(fileHistory);
    }

}

