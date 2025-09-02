using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using static MenuMetrics;
using static UIManager;

// This script has all initializations of the left side of the UI
// If you want to edit:
//                  Title
//                  Symbol
//                  Related Works
//                  Process
//                  Next Arrow

public class RightUI : MonoBehaviour
{
    // This UIDocument gets the components in Assets/GUI/Script/
    // Where:
    //  Info is the position of GUI components
    //  ButtonCSS is the CSS styling of the components
    public UIDocument uiDocument;

    // This initializes an object of UIManager
    private UIManager uiManager;

    // These are the text files to populate the boxes
    [SerializeField] string fileSymbolism, fileWorks, fileTechniques;

    // These are the UI elements:
    // Containers
    private Button nextButton, symbolButton, relatedButton, processButton;
    private ScrollView titleContainer;

    // Text Headers
    private Label titleHeader, rightHeader, titleText, rightText;

    void Start()
    {
        //populate contents so its not left with placeholder text
        uiManager.populate("Title", "", titleHeader, titleText);

    }
    void OnEnable()
    {
        // This is every component of the UI, set references to each
        var root = uiDocument.rootVisualElement;

        nextButton = root.Q<Button>("Forward");
        titleContainer = root.Q<ScrollView>("Title");
        symbolButton = root.Q<Button>("Symbol");
        relatedButton = root.Q<Button>("Related");
        processButton = root.Q<Button>("Process");
   
        titleHeader = root.Q<Label>("TitleHeader");
        rightHeader = root.Q<Label>("RightHeader");

        titleText = root.Q<Label>("TitleText");
        rightText = root.Q<Label>("RightText");


        //If Forward button is pressed, we can enter the menu
        nextButton.clicked += uiManager.EnterView;
        // If One of the buttons on the Right UI is pressed
        symbolButton.clicked += SymbolView;
        relatedButton.clicked += RelatedView;
        processButton.clicked += ProcessView;
    }

    //EVENTS THAT OCCUR FOR SELECTING WHICH UI IS ENABLED
    // What happens if symbol is pressed:
    void SymbolView()
    {
        //increment metrics click
        MenuMetrics.IncrementClick("Symbolism");
        //increment counter (EX: page 2) and sets status
        uiManager.IncrementCounter();

        TimeLog("enter module menus");
        SetStatus("Symbolism");
        setStartTime();

        Debug.Log("symbol");

        //populate text field with Symbol text contents
        rightHeader.text = "Symbolism";
        rightText.text = uiManager.LoadTextFromFile(fileSymbolism);

        uiManager.displayNone("menu");
        uiManager.displayFlex("backMenu");
        uiManager.displayFlex("rightMenu");
        uiManager.displayNone("startMenu");
    }

    void RelatedView()
    {
        //increment metrics click
        MenuMetrics.IncrementClick("Related");
        //increment counter (EX: page 2) and sets status
        uiManager.IncrementCounter();

        TimeLog("enter module menus");
        SetStatus( "Related" );
        setStartTime();

        //populate text field with Symbol text contents
        rightHeader.text = "Related Works";
        rightText.text = uiManager.LoadTextFromFile(fileWorks);

        uiManager.displayNone("menu");
        uiManager.displayFlex("backMenu");
        uiManager.displayFlex("rightMenu");
        uiManager.displayNone("startMenu");
    }

    void ProcessView()
    {
        //increment metrics click
        MenuMetrics.IncrementClick("Process");

        //increment counter (EX: page 2) and sets status
        uiManager.IncrementCounter();
        TimeLog("enter module menus");
        SetStatus("Process");
        setStartTime();

        //populate text field with Symbol text contents
        rightHeader.text = "Process";
        rightText.text = uiManager.LoadTextFromFile(fileTechniques);

        uiManager.displayNone("menu");
        uiManager.displayFlex("backMenu");
        uiManager.displayFlex("rightMenu");
        uiManager.displayNone("startMenu");
    }


}

