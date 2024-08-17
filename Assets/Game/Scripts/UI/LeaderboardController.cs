using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] UIDocument document = null;
    [SerializeField] VisualTreeAsset customButtonTemplate = null;
    [SerializeField] StyleSheet customButtonStyles = null;

    // Reference to your Sprite Atlas
    public SpriteAtlas spriteAtlas;

    List<VisualElement> buttons = new List<VisualElement> ();

    VisualElement questionsContent = null;
    VisualElement root = null;
    void Start()
    {
        root = document.rootVisualElement;

        SetupHeaderQuestionsButtons();

        //Setup header images
        SetupComponentImage("headerLine", "headerLine");
        SetupComponentImage("headerBlueBanner", "headerBlueBanner");

        //Setup buttons images
        foreach (CustomButton button in buttons) 
            SetupComponentImage("headerBlueBanner", button.name);


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


    Sprite SetupComponentImage(string spriteName, string parentName = null)
    {
        var imageInstance = root.Q<Image>(parentName);
        Sprite sprite = spriteAtlas.GetSprite(spriteName);

        if (sprite != null)
            imageInstance.sprite = sprite;
        else
            Debug.LogError("Sprite component not found.");

        return sprite;
    }
}
