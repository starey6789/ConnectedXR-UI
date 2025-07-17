using UnityEngine;
using UnityEngine.UIElements;
using static MenuMetrics;

public class OpenBox : MonoBehaviour
{
    public UIDocument uiDocument;

    // These are the text files to populate the boxes
    public string fileDesc;
    public string fileHistory;
    public string fileTitle;
    public string fileSymbolism;
    public string fileWorks;
    public string fileTechniques;

    // These are the UI elements
    // Pages
    private VisualElement menu;
    private VisualElement startMenu;
    private VisualElement backMenu;
    private VisualElement rightMenu;

    // Containers
    private Button upButton;
    private Button backButton;
    private ScrollView descriptionContainer;
    private ScrollView historyContainer;
    private ScrollView titleContainer;
    private Button symbolButton;
    private Button relatedButton;
    private Button processButton;

    // Text Headers
    private Label descriptionHeader;
    private Label historyHeader;
    private Label titleHeader;
    private Label rightHeader;
    // Text
    private Label descriptionText;
    private Label historyText;
    private Label titleText;

    private Label rightText;

    // Counter to see what page we're on
    private int counter;

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
        // This is every component of the UI, set references to each
        var root = uiDocument.rootVisualElement;

        menu = root.Q<VisualElement>("MenuButtons");
        startMenu = root.Q<VisualElement>("ArrowForward");
        backMenu = root.Q<VisualElement>("BackButtons");
        rightMenu = root.Q<VisualElement>("RightButtons");

        upButton = root.Q<Button>("Forward");
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
        upButton.clicked += EnterView;
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

        //increment counter (EX: page 1)
        counter++;
    }

        //Reset to all the way back
    void ResetView()
    {
        if (counter >= 2)
        {
            menu.style.display = DisplayStyle.Flex;
            backMenu.style.display = DisplayStyle.Flex;
            rightMenu.style.display = DisplayStyle.None;
            startMenu.style.display = DisplayStyle.None;
            counter--;
        }
        else
        {
            menu.style.display = DisplayStyle.None;
            backMenu.style.display = DisplayStyle.None;
            rightMenu.style.display = DisplayStyle.None;
            startMenu.style.display = DisplayStyle.Flex;
            counter--;
        }
    }

    void SymbolView()
    {
        //increment metrics click
        MenuMetrics.IncrementClick("Symbolism");
        //increment counter (EX: page 2)
        counter++;

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
        //increment counter (EX: page 2)
        counter++;

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
        
        //increment counter (EX: page 2)
        counter++;

        //populate text field with Symbol text contents
        rightHeader.text = "Process";
        rightText.text = LoadTextFromFile(fileTechniques);

            menu.style.display = DisplayStyle.None;
            backMenu.style.display = DisplayStyle.Flex;
            rightMenu.style.display = DisplayStyle.Flex;
            startMenu.style.display = DisplayStyle.None;
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

    //populate UI so it doesn't have null text

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

