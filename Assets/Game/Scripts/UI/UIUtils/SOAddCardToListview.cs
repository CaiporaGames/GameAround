using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GABackend.MKRLeaderboard;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "Add Card Listview", menuName = "Scriptable Objects / Add Card To List")]
public class SOAddCardToListview : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] VisualTreeAsset cardTemplate = null;
    [SerializeField] StyleSheet cardStyles = null;
    [SerializeField] SOImageSetup leaderBoardImageSetup = null;
    public SpriteAtlas atlas;

    [Header("List Card Setup")]
    [SerializeField] float cardMarginBottom = 40f;
    [SerializeField] float cardPaddingLeft = 220f;
    [SerializeField] float cardFixedItemHeight = 90;
    public void AddCardToLeaderboard(VisualElement root, string parentName, List<LeaderBoardItem> cardsData)
    {
        ListView listView = root.Q<ListView>(parentName);

        if (cardTemplate == null || listView == null)
        {
            Debug.LogWarning("Card Template or ListView is not assigned.");
            return;
        }

        if (cardsData.Count <= 0)
        {
            listView.Rebuild();
            return;
        }

        listView.itemsSource = cardsData;
        // Define how to create and bind items in the ListView
        listView.makeItem = () =>
        {
            // Create a new VisualElement using the card template
            var card = cardTemplate.Instantiate();
            card.styleSheets.Add(cardStyles);

            //This should be refactored to make this class more reusable
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
            cardElement.style.marginBottom = cardMarginBottom;
            cardElement.style.paddingLeft = cardPaddingLeft;

            scoreLabel.text = cardData.score.ToString();
        };

        listView.fixedItemHeight = cardFixedItemHeight;
        listView.style.height = cardFixedItemHeight * cardsData.Count;

        // Refresh the ListView to reflect the added item
        listView.Rebuild();
    }
}
