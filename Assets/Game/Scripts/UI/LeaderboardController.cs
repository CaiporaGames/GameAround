using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] UIDocument document = null;
    [SerializeField] VisualTreeAsset customButtonTemplate = null;
    [SerializeField] StyleSheet customButtonStyles = null;

    [SerializeField] VisualTreeAsset cardTemplate = null;
    [SerializeField] StyleSheet cardStyles = null;
    [SerializeField] SOImageSetup leaderBoardImageSetup = null;

    // Reference to your Sprite Atlas

    List<VisualElement> buttons = new List<VisualElement> ();

    VisualElement questionsContent = null;
    VisualElement root = null;

    private List<CardData> cardDataList;
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

        //Add card to list
        AddCardToLeaderboard("leaderboardList");
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

    private void AddCardToLeaderboard(string parentName)
    {
        cardDataList = new List<CardData>();

        for (int i = 0; i < 5; i++)
        {
            var newCard = new CardData { name = $"New Player {i + 1}", score = Random.Range(0, 100) };
            cardDataList.Add(newCard);
        }


        ListView listView = root.Q<ListView>(parentName);
        listView.itemsSource = cardDataList;

        if (cardTemplate == null || listView == null)
        {
            Debug.LogWarning("Card Template or ListView is not assigned.");
            return;
        }

        // Define how to create and bind items in the ListView
        listView.makeItem = () =>
        {
            // Create a new VisualElement using the card template
            var card = cardTemplate.Instantiate();
            card.styleSheets.Add(cardStyles);
            return card;
        };

        listView.bindItem = (element, index) =>
        {
            var cardElement = element as VisualElement;

            // Get the data for this item
            var cardData = cardDataList[index];

            // Bind the data to the UI elements (assuming you have some labels or fields in your template)
            var nameLabel = cardElement.Q<Label>("cardPositionValue");
            nameLabel.text = cardData.name;

            var scoreLabel = cardElement.Q<Label>("cardScore");
            scoreLabel.text = cardData.score.ToString();
        };

        // Refresh the ListView to reflect the added item
        listView.Rebuild();
    }
}
public class CardData
{
    public string name;
    public int score;
}