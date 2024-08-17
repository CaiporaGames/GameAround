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

    // The name of the sprite you want to use from the atlas
    public string spriteName;

    VisualElement questionsContent = null;
    VisualElement root = null;
    void Start()
    {
        root = document.rootVisualElement;
        SetupHeaderQuestionsButtons();
    }

    void SetupHeaderQuestionsButtons()
    {
        var buttonsContainer = root.Q<VisualElement>("leaderboardButtonsContainer");

        root.styleSheets.Add(customButtonStyles);

        AddCustomButton(buttonsContainer, "Daily", "", true);
        AddCustomButton(buttonsContainer, "Weekly");
        AddCustomButton(buttonsContainer, "Monthly");
    }

    private void AddCustomButton(VisualElement parent, string buttonName, string className = "",  bool isActive = false)
    {
        var customButtonRoot = customButtonTemplate.CloneTree();

        var customButtonInstance = new CustomButton();
        customButtonInstance.SetButtonProperties(buttonName, className);

        if (!isActive)
            customButtonInstance.AddAdditionalClass("inactiveButton");

        parent.styleSheets.Add(customButtonStyles);
        parent.Add(customButtonInstance);
    }

    void SetupLeaderboardBackground()
    {
        VisualElement leaderboardHeader = root.Q<VisualElement>("leaderboardHeader");
        var headerImage = leaderboardHeader.Q<Image>("headerImage");

        // Load the sprite from the atlas
        Sprite sprite = spriteAtlas.GetSprite(spriteName);

        if (sprite != null && headerImage != null)
        {
            // Convert the sprite to a Texture2D
            Texture2D texture = SpriteToTexture(sprite);

            // Assign the texture to the UI Toolkit Image
            headerImage.image = texture;
        }
        else
        {
            Debug.LogError("Sprite or Image component not found.");
        }
    }

    // Helper method to convert Sprite to Texture2D
    private Texture2D SpriteToTexture(Sprite sprite)
    {
        if (sprite == null)
            return null;

        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        Color[] pixels = sprite.texture.GetPixels(
            (int)sprite.textureRect.x,
            (int)sprite.textureRect.y,
            (int)sprite.textureRect.width,
            (int)sprite.textureRect.height
        );
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
}
