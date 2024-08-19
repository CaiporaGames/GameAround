using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using static GABackend.MKRLeaderboard;

public class LeaderboardManager : MonoBehaviour
{
    [Header("UI Document Settings")]
    [SerializeField] UIDocument document = null;
    [SerializeField] VisualTreeAsset customButtonTemplate = null;
    [SerializeField] StyleSheet customButtonStyles = null;

    [Header("Dependency Injection")]
    [SerializeField] SOImageSetup leaderBoardImageSetup = null;
    [SerializeField] SODatabaseCRUD databaseCRUD = null;
    [SerializeField] SOAddCardToListview addCardToLeaderboard = null;

    List<VisualElement> buttons = new List<VisualElement> ();
    List<LeaderBoardItem> cardsData = new List<LeaderBoardItem>();
    VisualElement root = null;


    void Start()
    {
        //get ui document
        root = document.rootVisualElement;

        SetupHeaderQuestionsButtons();

        //Setup header images
        leaderBoardImageSetup.SetupComponentImage(root, "headerLine", "headerLine");
        leaderBoardImageSetup.SetupComponentImage(root, "headerBlueBanner", "headerBlueBanner");

        //Setup buttons images
        foreach (CustomButton button in buttons)
            leaderBoardImageSetup.SetupComponentImage(button, "defaultButton", button.name);

        //start the leaderboard with daily leaderboard
        _ = HandleButtonClickAsync("Daily");

        //Setup daily button to active
        leaderBoardImageSetup.SetupComponentImage(root, "activeButton", "Daily");
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

        CustomButton.buttonClicked += HandleButtonClickAsync;

        // Apply the template to the custom button
        customButtonInstance.styleSheets.Add(customButtonStyles);
        customButtonTemplate.CloneTree(customButtonInstance);
        customButtonInstance.Q<Image>().name = buttonName;

        //Change button display name
        var label = customButtonInstance.Q<Label>("buttonLabelBackground");
        label.text = buttonName;

        // Add the stylesheet to the button, not the parent
        parent.Add(customButtonInstance);
        return customButtonInstance;
    }

    //This could be refactored to a factory class if more buttons will be added in the future
    public async Task AddCardToLeaderboard(string buttonName)
    {
        Tuple<List<LeaderBoardItem>, string> result = null;

        try
        {
            switch (buttonName)
            {
                case "Daily":
                    result = await databaseCRUD.GetTodayDailyLeaderboardAsync();
                    break;
                case "Weekly":
                    result = await databaseCRUD.GetTodayWeeklyLeaderboardAsync();
                    break;
                case "Monthly":
                    result = await databaseCRUD.GetTodayMonthlyLeaderboardAsync();
                    break;
                default:
                    Debug.LogError($"Invalid button name: {buttonName}");
                    return;
            }

            if (result.Item2 != null)
            {
                Debug.LogError($"Error fetching leaderboard: {result.Item2}");
                return;
            }

            //Clear cards data to avoid list repeated data
            cardsData.Clear();
            cardsData.AddRange(result.Item1);

            addCardToLeaderboard.AddCardToLeaderboard(root, "leaderboardList", cardsData);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return;
        }
    }

    async Task HandleButtonClickAsync(string buttonName)
    {
        await AddCardToLeaderboard(buttonName);

        //Change buttons background image between default
        leaderBoardImageSetup.SetupComponentImage(root, "defaultButton", "Daily");
        leaderBoardImageSetup.SetupComponentImage(root, "defaultButton", "Weekly");
        leaderBoardImageSetup.SetupComponentImage(root, "defaultButton", "Monthly");

        //Change buttons background image between active
        leaderBoardImageSetup.SetupComponentImage(root, "activeButton", buttonName);
    }

    private void OnDisable()
    {
        CustomButton.buttonClicked -= HandleButtonClickAsync;
    }
}
