using GABackend;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using static CustomButton;
using static GABackend.MKRLeaderboard;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] UIDocument document = null;
    [SerializeField] VisualTreeAsset customButtonTemplate = null;
    [SerializeField] StyleSheet customButtonStyles = null;

    [SerializeField] VisualTreeAsset cardTemplate = null;
    [SerializeField] StyleSheet cardStyles = null;
    [SerializeField] SOImageSetup leaderBoardImageSetup = null;
    [SerializeField] SODatabaseCRUD databaseCRUD = null;

    List<VisualElement> buttons = new List<VisualElement> ();

    VisualElement questionsContent = null;
    VisualElement root = null;


    void Start()
    {
        root = document.rootVisualElement;

        SetupHeaderQuestionsButtons();

        //Setup header images
        leaderBoardImageSetup.SetupComponentImage(root, "headerLine", "headerLine");
        leaderBoardImageSetup.SetupComponentImage(root, "headerBlueBanner", "headerBlueBanner");

        //Setup buttons images
        foreach (CustomButton button in buttons)
            leaderBoardImageSetup.SetupComponentImage(button, "defaultButton", button.name);

        //Setup List View image
        leaderBoardImageSetup.SetupComponentImage(root, "cardsBoard", "leaderboardBackground");

        AddCardToLeaderboard("Daily");
    }

    void SetupHeaderQuestionsButtons()
    {
        var buttonsContainer = root.Q<VisualElement>("leaderboardButtonsContainer");

        root.styleSheets.Add(customButtonStyles);

        buttons.Add(AddCustomButton(buttonsContainer, "Daily", "", true));
        buttons.Add(AddCustomButton(buttonsContainer, "Weekly"));
        buttons.Add(AddCustomButton(buttonsContainer, "Monthly"));
    }

    private VisualElement AddCustomButton(VisualElement parent, string buttonName, string className = null, bool isActive = false)
    {
        var customButtonInstance = new CustomButton();
        customButtonInstance.name = buttonName;
        customButtonInstance.SetButtonProperties(buttonName, className);
        CustomButton.buttonClicked += AddCardToLeaderboard;
        if (!isActive)
            customButtonInstance.AddAdditionalClass("inactiveButton");

        // Apply the template to the custom button
        customButtonInstance.styleSheets.Add(customButtonStyles);
        customButtonTemplate.CloneTree(customButtonInstance);
        customButtonInstance.Q<Image>().name = buttonName;
        var label = customButtonInstance.Q<Label>("buttonLabelBackground");
        label.text = buttonName;
        // Add the stylesheet to the button, not the parent
        parent.Add(customButtonInstance);
        return customButtonInstance;
    }

    void AddCardToLeaderboard(string buttonName)
    {
        Debug.Log("kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk: "+ buttonName);
        List<LeaderBoardItem> cardsData = new List<LeaderBoardItem>();
        if (buttonName == "Daily")
            for (int i = 0; i < databaseCRUD.GetTodayDailyLeaderboard().Item1.Count; i++)
            {
                LeaderBoardItem card = new LeaderBoardItem();

                card.position = databaseCRUD.GetTodayDailyLeaderboard().Item1[i].position;
                card.displayName = databaseCRUD.GetTodayDailyLeaderboard().Item1[i].displayName;
                card.score = databaseCRUD.GetTodayDailyLeaderboard().Item1[i].score;
                cardsData.Add(card);
            }
        else if (buttonName == "Weekly")
            for (int i = 0; i < databaseCRUD.GetTodayWeeklyLeaderboard().Item1.Count; i++)
            {
                LeaderBoardItem card = new LeaderBoardItem();
                card.position = databaseCRUD.GetTodayWeeklyLeaderboard().Item1[i].position;
                card.displayName = databaseCRUD.GetTodayWeeklyLeaderboard().Item1[i].displayName;
                card.score = databaseCRUD.GetTodayWeeklyLeaderboard().Item1[i].score;
                cardsData.Add(card);
            }
        else if (buttonName == "Monthly")
            for (int i = 0; i < databaseCRUD.GetTodayMonthlyLeaderboard().Item1.Count; i++)
            {
                LeaderBoardItem card = new LeaderBoardItem();

                card.position = databaseCRUD.GetTodayMonthlyLeaderboard().Item1[i].position;
                card.displayName = databaseCRUD.GetTodayMonthlyLeaderboard().Item1[i].displayName;
                card.score = databaseCRUD.GetTodayMonthlyLeaderboard().Item1[i].score;
                cardsData.Add(card);
            }
        AddCardToLeaderboard("leaderboardList", cardsData);
    }

    private void AddCardToLeaderboard(string parentName, List<LeaderBoardItem> cardsData)
    {
        ListView listView = root.Q<ListView>(parentName);
        if (cardTemplate == null || listView == null)
        {
            Debug.LogWarning("Card Template or ListView is not assigned.");
            return;
        }
        listView.Clear();

        listView.itemsSource = cardsData;
        // Define how to create and bind items in the ListView
        listView.makeItem = () =>
        {
            // Create a new VisualElement using the card template
            var card = cardTemplate.Instantiate();
            card.styleSheets.Add(cardStyles);

            leaderBoardImageSetup.SetupComponentImage(card, "defaultCard", "cardImage");
            leaderBoardImageSetup.SetupComponentImage(card, "positionBackground", "cardPositionBackground");
            leaderBoardImageSetup.SetupComponentImage(card, "AppIcon", "cardDriverIcon");
            card.style.flexGrow = 1;
            return card;
        };

        listView.bindItem = (element, index) =>
        {
            var cardElement = element as VisualElement;

            // Get the data for this item
            var cardData = cardsData[index];

            // Bind the data to the UI elements (assuming you have some labels or fields in your template)
            var positionLabel = cardElement.Q<Label>("cardPositionValue");
            positionLabel.text = cardData.position.ToString();

            var nameLabel = cardElement.Q<Label>("cardDriverName");
            nameLabel.text = cardData.displayName.ToString();


            var scoreLabel = cardElement.Q<Label>("cardScore");
            scoreLabel.text = cardData.score.ToString();

            cardElement.style.marginBottom = 20;
            cardElement.style.paddingLeft = 220;

            scoreLabel.text = cardData.score.ToString();
        };

        listView.fixedItemHeight = 90;
        // Refresh the ListView to reflect the added item
        listView.Rebuild();
    }

    private void OnDisable()
    {
        CustomButton.buttonClicked -= AddCardToLeaderboard;
    }
}
